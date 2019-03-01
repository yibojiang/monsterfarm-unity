using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAppleDealer : Interact {

    public override void InteractAction() {
        var dm = DialogManager.Instance;
        var allTexts = new List<string>();
		
        var pc = PlayerController.Instance;

        if (pc.Coins >= 100)
        {
            allTexts.Add("Are you willing to buy an apple for $100 ?");
            allTexts.Add("Deal !");
            dm.SetText(allTexts, () => {
                pc.AddCoins(-100);
            });	
        }
        else {
            allTexts.Add("You don't have money to buy my apple, it costs $100.");
            dm.SetText(allTexts);
        }
        
        dm.Show();
    }
}
