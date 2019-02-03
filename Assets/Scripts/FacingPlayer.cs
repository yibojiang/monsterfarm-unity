using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingPlayer : MonoBehaviour {
	private PlayerController pc_;
	// Use this for initialization
	void Awake () {
		pc_ = PlayerController.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		var scale = transform.localScale;
		if (transform.position.x < pc_.transform.position.x) {
			scale.x = Mathf.Abs(scale.x);
		}
		else {
			scale.x = -Mathf.Abs(scale.x);
		}
		transform.localScale = scale;
	}
}
