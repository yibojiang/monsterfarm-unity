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
		private SpriteRenderer sprite;
		private Animator animSM_;
		private Vector3 lastMovingVel_;
		private Rigidbody2D rigidBody;

		private bool _isWondering = false;
		// Use this for initialization

		void Awake()
		{
			sprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();
			animSM_ = sprite.gameObject.GetComponent<Animator>();
			rigidBody = this.GetComponent<Rigidbody2D>();
		}
		
		void Start ()
		{
			StartWondering();
		}

		public void StartWondering()
		{
			if (!_isWondering)
			{
				this.enabled = true;
				_isWondering = true;
				StartCoroutine("RandomWalkAction");
				var randomIdleStart = Random.Range(0,animSM_.GetCurrentAnimatorStateInfo(0).length); //Set a random part of the animation to start from
				animSM_.Play("Idle", 0, randomIdleStart);	
			}
		}

		public void StopWondering()
		{
			if (_isWondering)
			{
				this.enabled = false;
				StopCoroutine("RandomWalkAction");
			}
		}
	
		IEnumerator RandomWalkAction() {
			while (true) {
				movingVel = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
				movingVel.Normalize();
				animSM_.SetBool("IsAttacking", false);
				yield return new WaitForSeconds(Random.Range(0.5f, 0.6f));
				animSM_.SetBool("IsAttacking", true);
				movingVel = Vector2.zero;
				yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
				animSM_.SetBool("IsAttacking", false);
			}
		}
		
		// Update is called once per frame
		void FixedUpdate () {
			if (_isWondering)
			{
				movingVel = Vector2.ClampMagnitude(movingVel, maxSpeed);
				if (movingVel.magnitude > 0.1) {
					lastMovingVel_ = movingVel;
				}

				rigidBody.MovePosition(rigidBody.position + movingVel * Time.fixedDeltaTime);
				animSM_.SetFloat("DirectionX", lastMovingVel_.x);
				animSM_.SetFloat("DirectionY", lastMovingVel_.y);	
			}
		}
	}
}
