using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractNPC : Interact {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void InteractAction() {
		// Debug.Log("npc");
		var dm = DialogManager.Instance;
		var allTexts = new List<string>();
		
		var pc = PlayerPawn.Instance;

		if (pc.items.ContainsKey("egg") && pc.items["egg"] > 0) {
			allTexts.Add("Are you sure to sell the eggs for $100 ?");
			allTexts.Add("Deal !");
			dm.SetText(allTexts, () => {
				pc.AddCoins(1000);
				pc.items["egg"] -= 1;
			});	
		}
		else {
			allTexts.Add("Hi, young, advernturer,");
			allTexts.Add("If you have anything, you can sell it to me.");
			dm.SetText(allTexts);
		}
		
		dm.Show();
	}
}
