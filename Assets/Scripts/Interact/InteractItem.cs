using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem : Interact {
	public string itemName;
	public int itemCount;
	// Use this for initialization
	void Start () {
		
	}
	
	public override void InteractAction() {
		var pc = PlayerController.Instance;
		if (!pc.items.ContainsKey(itemName)) {
			pc.items[itemName] = 0;
		}
		pc.items[itemName] += itemCount;
		if (transform.parent != null)
		{
			Destroy(this.transform.parent.gameObject);	
		}
		else
		{
			Destroy(this.gameObject);
		}
		
	}
}
