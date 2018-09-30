using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public SpriteRenderer sprite_;
	public float maxMovingSpeed = 1.0f;
	private Vector3 movingVel;
	private Animator animSM_;
	// private bool faceingRight_ = false;
	public float acc = 3f;
	public float drag = 10f;

	// Use this for initialization
	void Start () {
		// sprite_ = this.gameObject.GetComponent<SpriteRenderer>();
		animSM_ = sprite_.gameObject.GetComponent<Animator>();
	}

	void Update() {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var playerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
		// Debug.Log(playerInput);
		
		playerInput = Vector3.Normalize(playerInput);

		movingVel += playerInput * acc;

		if (movingVel.magnitude > 0.1) {
			movingVel -= movingVel * drag;
			movingVel = Vector3.ClampMagnitude(movingVel, maxMovingSpeed);
		}
		else {
			movingVel = Vector3.zero;
		}
		// Debug.Log(movingVel);

		transform.position = transform.position + movingVel * Time.fixedDeltaTime;
		animSM_.SetFloat("Speed", movingVel.magnitude);
		// Debug.Log(movingVel.magnitude);
		if (movingVel.x > 0)
			sprite_.flipX = true;
		else if (movingVel.x < 0)
			sprite_.flipX = false;

		// if (faceingRight_)
		// if (movingVel.x)
		// sprite_.flipX = faceingRight_;
		// movingSpeed += 
		// Debug.Log(Input.GetAxis("Horizontal"));
		// Debug.Log(Input.GetAxis("Vertical"));
	}
}
