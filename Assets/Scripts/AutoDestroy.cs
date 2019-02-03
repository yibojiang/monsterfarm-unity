using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {
	public float lifeTime = 1f;
	private float life_ = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (life_ < lifeTime) {
			life_ += Time.deltaTime;
		}
		else {
			Destroy(this.gameObject);
		}
	}
}
