using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class FacingPlayer : MonoBehaviour {
	private PlayerPawn player_;
	// Use this for initialization
	void Awake ()
	{
		player_ = PlayerController.Instance.playerPawn;
	}
	
	// Update is called once per frame
	void Update () {
		var scale = transform.localScale;
		if (transform.position.x < player_.transform.position.x) {
			scale.x = Mathf.Abs(scale.x);
		}
		else {
			scale.x = -Mathf.Abs(scale.x);
		}
		transform.localScale = scale;
	}
}
