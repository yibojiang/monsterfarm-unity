using System.Collections;
using System.Collections.Generic;
using MonsterFarm;
using Pathfinding;
using UnityEngine;

public class MonsterPawn : MobPawn {
	public AudioClip hitClip;
	private bool _isFollowing = false;
	private Seeker _seeker;
	public Transform _target;
	public Vector3 followOffset = new Vector3(0.2f, 0.1f,0 );
	private IAstarAI _ai;
	public bool canFeed;
	public string favouriteItem;
	public int Friendship { get; protected set; }
	public int Age;
	public RandomWalk behaviorWonder;
	public void GetHit (Vector3 pos) {
		var hurtParticlePrefab = Resources.Load("Prefab/Effect_BloodHit");
		var hurtPS = (GameObject)GameObject.Instantiate(hurtParticlePrefab, pos, Quaternion.identity);
		var am = AudioManager.Instance;
		am.PlaySFX(hitClip);
	}

	void OnEnable () {
		_ai = GetComponent<IAstarAI>();
		if (_ai != null) _ai.onSearchPath += Update;
	}

	void OnDisable () {
		if (_ai != null) _ai.onSearchPath -= Update;
	}
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(RandomOffsetCoroutine());
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

	IEnumerator RandomOffsetCoroutine()
	{
		float timer = 0f;
		float interval = 3.0f;
		while (true)
		{
			timer += Time.deltaTime;
			if (timer > interval)
			{
				timer -= interval;
				interval = Random.Range(2f, 6f);
				float len = 0.5f;
				float rad = Random.Range(0, 2 * Mathf.PI);
				followOffset = new Vector3(len * Mathf.Cos(rad), len * Mathf.Sin(rad), 0);
			}
			yield return new WaitForEndOfFrame();
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if (_ai != null && _target != null)
		{
			_ai.destination = _target.position + followOffset;
		}
	}

	public void FollowTarget(Transform target)
	{
		_isFollowing = true;
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
		_target = null;
		
		if (behaviorWonder != null)
		{
			behaviorWonder.StartWondering();
		}
	}
}
