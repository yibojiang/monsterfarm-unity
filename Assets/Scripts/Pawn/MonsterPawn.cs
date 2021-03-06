﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MonsterFarm;
using Pathfinding;
using UnityEngine;
using Crossbone.AI.BehaviorTree;

public enum AIType
{
	WonderAndChase,
	Wonder,
}
public class MonsterPawn : MobPawn {
	public Vector3 followOffset = new Vector3(0.2f, 0.1f,0 );
	public string favouriteItem;
	public Transform _target;
	public AudioClip hitClip;
	private Seeker _seeker;
	private IAstarAI _ai;
	
	[CanBeNull] private RandomWalk behaviorWonder;
	[CanBeNull] private AIPath _aiPath;
	
	private Animator _animSM;
	public int Friendship { get; private set; }
	public int Age;
	public bool canFeed;
	private bool _isFollowing = false;
	private BehaviorTree _bt;
	
	private int _idIsAttacking = 0;
	private int _idDirectionX = 0;
	private int _idDirectionY = 0;
	private int _idIdle = 0;

	public AIType aiType;
	public float normalSpeed = 1f;

	public bool IsFollowing()
	{
		return _isFollowing;
	}

	public void GetHit (Vector3 pos, int damage) {
		var fxBloodhitPrefab = Resources.Load("Prefab/fx_bloodhit");
		GameObject.Instantiate(fxBloodhitPrefab, pos, Quaternion.identity);
		var am = AudioManager.Instance;
		am.PlaySFX(hitClip);
		Hurt(pos, damage);
		_bt.SetKeyValue("chase_target", PlayerController.Instance.playerPawn);
	}

	protected void Awake()
	{
		base.Awake();
		behaviorWonder = GetComponent<RandomWalk>();
		_aiPath = GetComponent<AIPath>();
		_ai = GetComponent<IAstarAI>();
		_animSM = _sprite.gameObject.GetComponent<Animator>();
		_seeker = GetComponent<Seeker>();
		
		foreach (var p in _animSM.parameters)
		{
			if (p.name == "IsAttacking")
			{
				_idIsAttacking = Animator.StringToHash("IsAttacking");
			}
			else if (p.name == "DirectionX")
			{
				_idDirectionX = Animator.StringToHash("DirectionX");	
			}
			else if (p.name == "DirectionX")
			{
				_idDirectionY = Animator.StringToHash("DirectionY");	
			}
		}

		_idIdle = Animator.StringToHash("Idle");
		if (!_animSM.HasState(0, _idIdle))
		{
			_idIdle = 0;
		}
	}

	private void Start()
	{
		base.Start();
		if (aiType == AIType.Wonder)
		{
			_bt = BehaviorTree.CreateWonderBehavior(this);	
		}
		else if (aiType == AIType.WonderAndChase)
		{
			_bt = BehaviorTree.CreateWonderChaseBehavior(this);
		}
		
		_bt.Run();
	}

	public virtual void AddAge()
	{
		Age++;
	}

	public void AddFriendShip(int friendship)
	{
		Friendship += friendship;
		if (Friendship >= 1)
		{
			FollowTarget(PlayerController.Instance.playerPawn.transform);
			PlayerController.Instance.playerPawn.AddFollower(this);
		}
	}

	public override void SetDestination(Vector3 targetPosition, float minDist, float speedFactor)
	{
		_aiPath.endReachedDistance = minDist;
		_ai.destination = targetPosition;
		_ai.SearchPath();
		_ai.maxSpeed = normalSpeed * speedFactor;
	}

	public override bool DestinationReached()
	{;
		return _ai.reachedDestination;
	}

	public override bool DestinationCannotReached()
	{
		
		return !_ai.pathPending && _ai.reachedEndOfPath;
	}

	// Update is called once per frame
	void Update () {

		if (_ai != null && _target && !_ai.pathPending)
		{
			SetDestination(_target.position, 0.5f, 1.3f);
		}
		
		if (_idDirectionX != 0)
		{
			_animSM.SetFloat(_idDirectionX, _ai.velocity.x);	
		}
				
		if (_idDirectionY != 0)
		{
			_animSM.SetFloat(_idDirectionY,_ai.velocity.y);
		}

		if (_idIsAttacking != 0)
		{
			if (_animSM.GetBool(_idIsAttacking) == true)
			{
				_animSM.SetBool(_idIsAttacking, false);	
			}

			if (_bt.HasKey("chase_target"))
			{
				var chaseTarget = _bt.GetValue<Transform>("chase_target");
				
				if (chaseTarget)
				{
					var dist = chaseTarget.position - transform.position;
//					Debug.Log(dist.magnitude);
					if (dist.sqrMagnitude < 12f)
					{
//						Debug.Log("attack");
						_animSM.SetBool(_idIsAttacking, true);
					}	
				}
			}	
		}
	}

	public void FollowTarget(Transform target)
	{
		_isFollowing = true;
		_bt.Stop();
		_target = target;
	}

	public void UnFollowTarget()
	{
		_isFollowing = false;
		_target = null;
		
		_bt.Run();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_bt.SetKeyValue("chase_target", PlayerController.Instance.playerPawn.transform);
		}
	}
}
