using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MonsterFarm;
using Pathfinding;
using UnityEngine;

public class MonsterPawn : MobPawn {
	public Vector3 followOffset = new Vector3(0.2f, 0.1f,0 );
	public string favouriteItem;
	public Transform _target;
	public AudioClip hitClip;
	private Seeker _seeker;
	private IAstarAI _ai;
	
	[CanBeNull] private RandomWalk behaviorWonder;
	[CanBeNull] private AIPath _aiPath;
	public int Friendship { get; private set; }
	public int Age;
	public bool canFeed;
	private bool _isFollowing = false;

	public bool IsFollowing()
	{
		return _isFollowing;
	}

	public void GetHit (Vector3 pos, int damage) {
		var fxBloodhitPrefab = Resources.Load("Prefab/fx_bloodhit");
		GameObject.Instantiate(fxBloodhitPrefab, pos, Quaternion.identity);
		var am = AudioManager.Instance;
		am.PlaySFX(hitClip);
		Hurt(damage);
	}

	void OnEnable () {
		_ai = GetComponent<IAstarAI>();
		if (_ai != null) _ai.onSearchPath += Update;
	}

	void OnDisable () {
		if (_ai != null) _ai.onSearchPath -= Update;
	}

	protected void Awake()
	{
		base.Awake();
		behaviorWonder = GetComponent<RandomWalk>();
		_aiPath = GetComponent<AIPath>();
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
	
	// Update is called once per frame
	void Update () {

		if (_ai != null && _target)
		{
			_ai.destination = _target.position;
			var dist = _target.position - transform.position;
			if (dist.magnitude < 0.5f)
			{
				_ai.destination = transform.position;
			}
		}
	}

	public void FollowTarget(Transform target)
	{
		_isFollowing = true;
		_aiPath.canMove = true;
		if (!_seeker)
		{
			_seeker = gameObject.AddComponent<Seeker>();	
		}

		_target = target;
		if (behaviorWonder != null)
		{
			behaviorWonder.StopWondering();
		}
	}

	public void UnFollowTarget()
	{
		_isFollowing = false;
		_aiPath.canMove = false;
		_target = null;
		
		if (behaviorWonder != null)
		{
			behaviorWonder.StartWondering();
		}
	}
}
