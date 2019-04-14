using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
	public Transform target;
	public GameObject bulletPrefab;
	private float _shootTimer;
	public float shootInterval = 0.8f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			_shootTimer += Time.deltaTime;
			if (_shootTimer > shootInterval)
			{
				_shootTimer -= shootInterval;
				Shoot(target.position);
			}
		}
	}

	public void Shoot(Vector3 pos)
	{
		var bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		var bullet = bulletObj.GetComponent<Bullet>();
		var dir = target.position - transform.position;
		bullet.SetVel(dir.normalized * 10f);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			target = other.transform;	
		}
		//var bulletPrefab =Resources.Load("Prefab/")
		//var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
	}
}
