using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	private Vector3 movingVel_;
	private float lifeTime_ = 0.5f;
	private float life_;
	private bool hasShoot_ = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (hasShoot_) {
			life_ += Time.deltaTime;
			if (life_ > lifeTime_) {
				Destroy(this.gameObject);
			}
		}
	}

	public void Shoot(Vector3 movingVel) {
		hasShoot_ = true;
		movingVel_ = movingVel;
		movingVel_.z = 0;
		movingVel_.Normalize();
	}

	void FixedUpdate () {
		transform.position = transform.position + movingVel_ * Time.fixedDeltaTime * 20f;
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.CompareTag("Monster")) {
			var monster = col.gameObject.GetComponent<MonsterPawn>();
			var contact = col.GetContact(0);
			monster.GetHit(new Vector3(contact.point.x, contact.point.y, 0));
			Destroy(this.gameObject);
		}
	}
}
