using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalWalk : MonoBehaviour {
	public Vector3 movingVel;
	public float maxSpeed = 5f;
	private Animator animSM_;
	public SpriteRenderer sprite;
	private Vector3 lastMovingVel_;
	public int dir = 1;

	void Awake() {
		animSM_ = sprite.gameObject.GetComponent<Animator>();
		StartCoroutine("RandomAction");
	}

	IEnumerator RandomAction() {
		while (true) {
			animSM_.SetBool("IsAttacking", false);
			// movingVel = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0f);
			movingVel = new Vector3(1, 0, 0) * dir;
			movingVel = Vector3.Normalize(movingVel) * Random.Range(0.7f, 1.0f);
			// Debug.Log("walk");
			yield return new WaitForSeconds(Random.Range(1.0f, 2.5f));
			// Debug.Log("idle");
			animSM_.SetBool("IsAttacking", true);
			// movingVel = Vector3.zero;

			yield return new WaitForSeconds(Random.Range(1.0f, 1.0f));
			animSM_.SetBool("IsAttacking", false);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
