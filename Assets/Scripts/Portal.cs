using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public Transform targetAnchor;
	public Vector3 offset = new Vector3(0, -0.3f, 0);
	public bool toOutDoor;

	void OnTriggerEnter2D(Collider2D col) {
		// Debug.Log(col.gameObject.name);
		if (col.CompareTag("Player")) {
			var playerPawn = col.GetComponent<PlayerPawn>();
			var body = col.gameObject.GetComponent<Rigidbody2D>();
			//Debug.Log(body.gameObject.name);
			// Not working
			body.MovePosition(targetAnchor.position);
			//body.transform.position = targetAnchor.position + offset;
			if (toOutDoor) {
				playerPawn.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
			}
			else {
				playerPawn.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}
}
