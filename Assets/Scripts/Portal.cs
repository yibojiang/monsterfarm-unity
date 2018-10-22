using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public Transform targetAnchor;
	public bool toOutDoor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		// Debug.Log(col.gameObject.name);
		if (col.CompareTag("Player")) {
			var playerCon = col.GetComponent<PlayerController>();
			var body = col.gameObject.GetComponent<Rigidbody2D>();
			Debug.Log(body.gameObject.name);
			// Not working
			// body.MovePosition(targetAnchor.position);
			body.transform.position = targetAnchor.position;
			if (toOutDoor) {
				playerCon.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
			}
			else {
				playerCon.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}
}
