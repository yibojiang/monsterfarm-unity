using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSlimeDealer : Interact {

    public override void InteractAction() {
        var dm = DialogManager.Instance;
        var allTexts = new List<string>();
		
        var pc = PlayerController.Instance;

        var player = pc.playerPawn;

        if (player.Followers.Count > 0)
        {
            allTexts.Add("Are you sure to sell the slime for $300 ?");
            allTexts.Add("Deal !");
            dm.SetText(allTexts, () => {
                pc.AddCoins(300);
                var mosnter = player.Followers[player.Followers.Count - 1];
                player.Followers.Remove(mosnter);
                Destroy(mosnter.gameObject);
            });	
        }
        else {
            allTexts.Add("Hi, young, advernturer,");
            allTexts.Add("If you have any slimes, you can sell it to me.");
            dm.SetText(allTexts);
        }
        
        dm.Show();
    }
}
