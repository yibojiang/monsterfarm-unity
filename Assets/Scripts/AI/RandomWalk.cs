using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;

namespace MonsterFarm
{
	public class RandomWalk : MonoBehaviour {
		public Vector2 movingVel;
		public float maxSpeed = 5f;
		
		private SpriteRenderer _sprite;
		private Animator _animSM;
		private Vector3 _lastMovingVel;
		private Rigidbody2D _rigidBody;

		private int _idIsAttacking = 0;
		private int _idDirectionX = 0;
		private int _idDirectionY = 0;
		private int _idIdle = 0;

		private bool _isWondering = false;

		private MobPawn _mob;
		// Use this for initialization

		void Awake()
		{
			_sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
			_animSM = _sprite.gameObject.GetComponent<Animator>();
			_rigidBody = GetComponent<Rigidbody2D>();
			_mob = GetComponent<MobPawn>();
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
		
		void Start ()
		{
//			StartWondering();
		}

		public void StartWondering()
		{
			if (!_isWondering)
			{
				enabled = true;
				_isWondering = true;
				StartCoroutine("RandomWalkAction");
				var randomIdleStart = Random.Range(0,_animSM.GetCurrentAnimatorStateInfo(0).length); //Set a random part of the animation to start from
				if (_idIdle != 0)
				{
					_animSM.Play(_idIdle, 0, randomIdleStart);	
				}
			}
		}

		public void StopWondering()
		{
			if (_isWondering)
			{
				enabled = false;
				StopCoroutine("RandomWalkAction");
			}
		}
	
		IEnumerator RandomWalkAction() {
			while (true) {
				movingVel = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
				movingVel.Normalize();
				if (_idIsAttacking != 0)
				{
					_animSM.SetBool(_idIsAttacking, false);	
				}
				
				yield return new WaitForSeconds(Random.Range(0.5f, 0.6f));

				if (_idIsAttacking != 0)
				{
					_animSM.SetBool(_idIsAttacking, true);	
				}
				
				movingVel = Vector2.zero;
				yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

				if (_idIsAttacking != 0)
				{
					_animSM.SetBool(_idIsAttacking, false);
				}
				yield return new WaitForEndOfFrame();
			}
		}
		
		// Update is called once per frame
		void FixedUpdate () {
			if (_isWondering)
			{
				movingVel = Vector2.ClampMagnitude(movingVel, maxSpeed);
				if (movingVel.magnitude > 0.1) {
					_lastMovingVel = movingVel;
				}
				
				_mob.SetVel(movingVel);

				if (_idDirectionX != 0)
				{
					_animSM.SetFloat(_idDirectionX, _lastMovingVel.x);	
				}
				
				if (_idDirectionY != 0)
				{
					_animSM.SetFloat(_idDirectionY, _lastMovingVel.y);
				}
			}
		}
	}
}
