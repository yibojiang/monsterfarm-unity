using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : MonoBehaviour {
	public Vector3 movingVel;
	public float maxSpeed = 5f;
	public SpriteRenderer sprite;
	private Animator animSM_;
	private Vector3 lastMovingVel_;
	// Use this for initialization
	void Start () {
		animSM_ = sprite.gameObject.GetComponent<Animator>();
		StartCoroutine("RandomWalkAction");
		var anim = sprite.gameObject.GetComponent<Animator>();
		var randomIdleStart = Random.Range(0,anim.GetCurrentAnimatorStateInfo(0).length); //Set a random part of the animation to start from
		Debug.Log("randomIdleStart:" + randomIdleStart);
        anim.Play("Idle", 0, randomIdleStart);
	}

	IEnumerator RandomWalkAction() {
		while (true) {
			movingVel = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0f);
			movingVel = Vector3.Normalize(movingVel);
			animSM_.SetBool("IsAttacking", false);
			// Debug.Log("walk");
			yield return new WaitForSeconds(Random.Range(0.5f, 0.6f));
			animSM_.SetBool("IsAttacking", true);
			// Debug.Log("idle");
			movingVel = Vector3.zero;
			yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
			animSM_.SetBool("IsAttacking", false);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		movingVel = Vector3.ClampMagnitude(movingVel, maxSpeed);
		if (movingVel.magnitude > 0.1) {
			lastMovingVel_ = movingVel;
		}
		transform.position += movingVel * Time.fixedDeltaTime;
		animSM_.SetFloat("DirectionX", lastMovingVel_.x);
		animSM_.SetFloat("DirectionY", lastMovingVel_.y);
	}
}
