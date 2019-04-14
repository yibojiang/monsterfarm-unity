using System.Collections;
using System.Collections.Generic;
using MonsterFarm;
using UnityEngine;

public class Arrow : MonoBehaviour {
	private Vector3 movingVel_;
	private float _lifeTime = 0.5f;
	private float _life;
	private bool hasShoot_ = false;
	private int _damage = 1;
	
	// Update is called once per frame
	void Update () {
		if (hasShoot_) {
			_life += Time.deltaTime;
			if (_life > _lifeTime) {
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
			monster.GetHit(new Vector3(contact.point.x, contact.point.y, 0), _damage);
			Destroy(this.gameObject);
		}
	}
}
