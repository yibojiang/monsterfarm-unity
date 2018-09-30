using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateDrawOrder : MonoBehaviour {
	private SpriteRenderer sprite_;
	// Use this for initialization
	void Start () {
		sprite_ = this.gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		sprite_.sortingOrder = (int)((100-transform.position.y) * 10);
	}
}
