using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFoodbow : InteractItem {
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    public override void InteractAction()
    {
        Debug.Log("InteractAction");
        var pc = PlayerController.Instance;
        if (pc.items.ContainsKey(itemName) && pc.items[itemName] + itemCount >= 0)
        {
            Debug.Log("InteractAction2");
            pc.items[itemName]+=itemCount;
        }
        else
        {
            var allTexts = new List<string>();
            var dm = DialogManager.Instance;
            allTexts.Add(string.Format("I don't have any {0}.", itemName));
            dm.SetText(allTexts);
            dm.Show();
        }
    }
}
