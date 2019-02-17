using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFoodbowl : InteractItem {
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    public override void InteractAction()
    {
        var dm = DialogManager.Instance;
        var pc = PlayerController.Instance;
        if (pc.items.ContainsKey(itemName) && pc.items[itemName] + itemCount >= 0)
        {
            pc.items[itemName]+=itemCount;
            var allTexts = new List<string>();
            allTexts.Add(string.Format("You put the {0} in the bowl.", itemName));
            dm.SetText(allTexts);
            dm.Show();
        }
        else
        {
            var allTexts = new List<string>();
            allTexts.Add(string.Format("I don't have any {0}.", itemName));
            dm.SetText(allTexts);
            dm.Show();
        }
    }
}
