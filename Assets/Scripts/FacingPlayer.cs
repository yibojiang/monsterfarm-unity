using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FacingPlayer : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		var scale = transform.localScale;
		if (transform.position.x <  PlayerController.Instance.playerPawn.transform.position.x) {
			scale.x = Mathf.Abs(scale.x);
		}
		else {
			scale.x = -Mathf.Abs(scale.x);
		}
		transform.localScale = scale;
	}
}
