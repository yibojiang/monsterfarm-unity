using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MonsterFarm;
using Pathfinding;
using UnityEngine;
using Crossbone.AI.BehaviorTree;

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
	}

	void OnEnable () {
//		if (_ai != null) _ai.onSearchPath += Update;
	}

	void OnDisable () {
//		if (_ai != null) _ai.onSearchPath -= Update;
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
		_bt = BehaviorTree.CreateWonderChaseBehavior(this);
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

	public override void SetDestination(Vector3 targetPosition, float minDist)
	{
//		_ai.destination = targetPosition;
//		_aiPath.canMove = true;
//		_aiPath.destination = targetPosition;
		_aiPath.endReachedDistance = minDist;
		_ai.destination = targetPosition;
		_ai.SearchPath();
	}

	public override bool DestinationReached()
	{
//		if (_aiPath.reachedEndOfPath)
//		{
//			_aiPath.canMove = false;
//		}
//		return _aiPath.reachedEndOfPath;
		return _ai.reachedDestination;
	}

	public override bool DestinationCannotReached()
	{
		
		return !_ai.pathPending && _ai.reachedEndOfPath;
	}

	// Update is called once per frame
	void Update () {
//		Debug.Log("pathPending: " + _ai.pathPending);
//		Debug.Log("reachedEndOfPath: " + _ai.reachedEndOfPath);
//		Debug.Log("reachedDestination: " + _ai.reachedDestination);

		if (_ai != null && _target && !_ai.pathPending)
		{
			SetDestination(_target.position, 0.5f);
		}
		
		if (_idDirectionX != 0)
		{
			_animSM.SetFloat(_idDirectionX, _ai.velocity.x);	
		}
				
		if (_idDirectionY != 0)
		{
			_animSM.SetFloat(_idDirectionY,_ai.velocity.y);
		}
	}

	public void FollowTarget(Transform target)
	{
		_isFollowing = true;
		_ai.maxSpeed = 1.5f;
		
		_bt.Stop();

		_target = target;
//		if (behaviorWonder != null)
//		{
//			behaviorWonder.StopWondering();
//		}
	}

	public void UnFollowTarget()
	{
		_isFollowing = false;
		_target = null;
		
		_bt.Run();
		
//		if (behaviorWonder != null)
//		{
//			behaviorWonder.StartWondering();
//		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
//		Debug.Log(other.gameObject.name);
		if (other.CompareTag("Player"))
		{
			_bt.SetKeyValue("chase_target", PlayerController.Instance.playerPawn.transform);
		}
	}
}
