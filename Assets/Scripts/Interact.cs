using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

	public string interactMessage = "Interact";

	private Collider2D collider2D_;

	private ContactFilter2D filter_;
	// Use this for initialization
	void Awake ()
	{
		collider2D_ = this.GetComponent<Collider2D>();
		filter_ = new ContactFilter2D();
		Debug.Log(LayerMask.NameToLayer("Player"));
		// filter_.SetLayerMask(LayerMask.NameToLayer("Player"));
		filter_.SetLayerMask(LayerMask.GetMask("Player"));
	}
	
	// Update is called once per frame
	void Update ()
	{
		var res = new Collider2D[1];
		
		collider2D_.OverlapCollider(filter_, res);
		foreach (var col in res)
		{
			if (col)
			{
				Debug.Log(col.name);
			}
		}
	}

	public virtual void InteractAction() {

	}

	public virtual void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Player")) {
			var pc = PlayerPawn.Instance;
			pc.interactTarget = this;
			pc.textInteract.text = interactMessage;
			pc.textInteract.gameObject.SetActive(true);
		}
	}

	public virtual void OnTriggerExit2D(Collider2D col) {
		if (col.CompareTag("Player")) {
			var pc = PlayerPawn.Instance;
			pc.interactTarget = null;
			pc.textInteract.text = "Interact";
			pc.textInteract.gameObject.SetActive(false);
		}
	}
}
