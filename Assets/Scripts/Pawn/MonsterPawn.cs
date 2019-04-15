using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MonsterFarm;
using Pathfinding;
using UnityEngine;
using Crossbone.AI.BehaviorTree;

public enum AIType
{
	WanderAndChase,
	Wander,
	Still,
}
public class MonsterPawn : MobPawn {
	public string favouriteItem;
	public Transform _target;
	public AudioClip hitClip;
	private Seeker _seeker;
	
	[CanBeNull] private AIPath _aiPath;
	
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
		_bt.SetKeyValue("chase_target", PlayerController.Instance.playerPawn.transform);
	}

	protected void Awake()
	{
		base.Awake();
		_aiPath = GetComponent<AIPath>();
		//_ai = GetComponent<IAstarAI>();
		_animSm = _sprite.gameObject.GetComponent<Animator>();
		_seeker = GetComponent<Seeker>();
		if (_aiPath != null)
		{
			_aiPath.enabled = false;
			_aiPath.enabled = true;	
		}

		//Debug.Log(_seeker.GetCurrentPath());
		if (_animSm != null)
		{
			foreach (var p in _animSm.parameters)
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
			if (!_animSm.HasState(0, _idIdle))
			{
				_idIdle = 0;
			}
		}
		

		
	}

	private void Start()
	{
		base.Start();
		if (aiType == AIType.Wander)
		{
			_bt = BehaviorTree.CreateWanderBehavior(this);
			_bt.Run();
		}
		else if (aiType == AIType.WanderAndChase)
		{
			_bt = BehaviorTree.CreateWanderChaseBehavior(this);
			_bt.Run();
		}
		else if (aiType == AIType.Still)
		{

		}
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
		var offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 1);
		_aiPath.destination = targetPosition + offset.normalized;
		_aiPath.SearchPath();
		_aiPath.maxSpeed = normalSpeed * speedFactor;
	}

	public override bool DestinationReached()
	{
		return _aiPath.reachedDestination;
	}

	public override bool DestinationCannotReached()
	{
		
		return !_aiPath.pathPending && _aiPath.reachedEndOfPath;
	}

	// Update is called once per frame
	void Update () {
		if (_aiPath != null && _target && !_aiPath.pathPending)
		{
			SetDestination(_target.position, 0.5f, 1.3f);
		}
		
		if (_idDirectionX != 0)
		{
			_animSm.SetFloat(_idDirectionX, _aiPath.velocity.x);	
		}
				
		if (_idDirectionY != 0)
		{
			_animSm.SetFloat(_idDirectionY,_aiPath.velocity.y);
		}

		if (_idIsAttacking != 0)
		{
			if (_animSm.GetBool(_idIsAttacking) == true)
			{
				_animSm.SetBool(_idIsAttacking, false);	
			}

			if (_bt.HasKey("chase_target"))
//			if (_chaseTarget)
			{
				Transform chaseTarget = _bt.GetValue<Transform>("chase_target");
//				Transform chaseTarget = (Transform)_bt._data["chase_target"];
				var dist = chaseTarget.position - transform.position;
				if (dist.sqrMagnitude < 12f)
				{
					_animSm.SetBool(_idIsAttacking, true);
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
			_bt.SetKeyValue("chase_target", other.transform);
		}
	}
}
