using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

	public string interactMessage = "Interact";

//	private Collider2D collider2D_;

//	private ContactFilter2D filter_;
	// Use this for initialization
	void Awake ()
	{
//		collider2D_ = this.GetComponent<Collider2D>();
//		filter_ = new ContactFilter2D();
//		filter_.SetLayerMask(LayerMask.GetMask("Player"));
	}
	/*
	// Update is called once per frame
	void Update ()
	{
		
		var col = GetOverlappingPawn();
		if (col != null)
		{
			var player = col.gameObject.GetComponent<Collider2D>();
			player.interactTarget = this;
			player.textInteract.text = interactMessage;
			player.textInteract.gameObject.SetActive(true);
		}
		
	}
	*/

	/*
	public PlayerPawn GetOverlappingPawn()
	{
		var res = new Collider2D[1];
		
		collider2D_.OverlapCollider(filter_, res);
		for (int i = 0; i < res.Length; i++)
		{
			var col = res[i];
			if (col && col.CompareTag("Player"))
			{
				return col.gameObject.GetComponent<MobPawn>();
			}
		}

		return null;
	}
	*/

	public virtual bool CanInteract()
	{
		return true;
	}

	public virtual void InteractAction() {
		
	}

	public virtual void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Player"))
		{
			var pawn = col.gameObject.GetComponent<MobPawn>();
			pawn.SetInteractTarget(this);
			pawn.interactTarget = this;
			//pawn.setInteractMessage(interactMessage);
			//pawn.textInteract.text = interactMessage;
			//pawn.textInteract.gameObject.SetActive(true);
		}
	}

	public virtual void OnTriggerExit2D(Collider2D col) {
		if (col.CompareTag("Player")) {
			var pawn = col.gameObject.GetComponent<MobPawn>();
			//var pc = PlayerPawn.Instance;
			//pc.interactTarget = null;
			pawn.SetInteractTarget(null);
			//pc.textInteract.text = "Interact";
			//pc.textInteract.gameObject.SetActive(false);
		}
	}
	
}
