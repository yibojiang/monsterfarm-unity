using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private float _life = 0;
	private float _lifeTime = 1f;
	private Vector3 _movingVel;
	public int damage = 1;
	public void SetVel(Vector3 vel)
	{
		_movingVel = vel;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		_life += Time.deltaTime;
		if (_life > _lifeTime) {
			Destroy(this.gameObject);
		}
	}
	
	void FixedUpdate () {
		transform.position = transform.position + _movingVel * Time.fixedDeltaTime;
	}
	
	void OnCollisionEnter2D (Collision2D col) {		
		if (col.gameObject.CompareTag("Player")) {
			var player = col.gameObject.GetComponent<PlayerPawn>();
			var contact = col.GetContact(0);
			player.Hurt(new Vector3(contact.point.x, contact.point.y, 0), damage);
			Destroy(this.gameObject);
		}
	}
}
