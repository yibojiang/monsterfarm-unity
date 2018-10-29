using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

	public string interactMessage = "Interact";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void InteractAction() {

	}

	public virtual void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Player")) {
			var pc = PlayerController.Instance;
			pc.interactTarget = this;
			pc.textInteract.text = interactMessage;
			pc.textInteract.gameObject.SetActive(true);
		}
	}

	public virtual void OnTriggerExit2D(Collider2D col) {
		if (col.CompareTag("Player")) {
			var pc = PlayerController.Instance;
			pc.interactTarget = null;
			pc.textInteract.text = "Interact";
			pc.textInteract.gameObject.SetActive(false);
		}
	}
}
