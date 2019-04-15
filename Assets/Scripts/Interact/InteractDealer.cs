using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDealer : Interact
{
	public string itemName;
	public int price;
	public int count;
	public TextMesh text;

	private void Start()
	{
		if (count > 0)
		{
			text.text = "$" + price;	
		}
		else
		{
			text.color = Color.red;
			text.text = "Sold out";
		}

	}

	// Use this for initialization
	public override void InteractAction() {
		var dm = DialogManager.Instance;
		var pc = PlayerController.Instance;
		var allTexts = new List<string>();
		if (count <= 0)
		{
			allTexts.Add("Sold out.");
			dm.SetText(allTexts);
		}
		else
		{
			if (pc.Coins >= price)
			{
				allTexts.Add(string.Format("Are you willing to buy the {0} for ${1} ?", itemName, price));
				dm.SetText(allTexts, () => {
					pc.AddCoins(-price);
					count--;
					pc.AddItemCount(itemName, 1);
					if (count == 0)
					{
						text.color = Color.red;
						text.text = "Sold out";
					}
				});	
			}
			else {
				allTexts.Add("You don't have enough money.");
				dm.SetText(allTexts);
			}	
		}
        
		dm.Show();
	}
}
