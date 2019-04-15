using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractChest : Interact {
	private SpriteRenderer _sprite;
	public Sprite closeSprite;

	public Sprite openSprite;
	public string itemName = "apple";
	public int count = 1;

	public bool opened = false;
	// Use this for initialization
	void Awake()
	{
		_sprite = GetComponent<SpriteRenderer>();
	}

	void Start()
	{
		if (opened)
		{
			_sprite.sprite = openSprite;
		}
	}

	public override bool CanInteract()
	{
		return !opened;
	}

	public override void InteractAction()
	{
		var dm = DialogManager.Instance;
		var allTexts = new List<string>();
		if (!opened)
		{
			_sprite.sprite = openSprite;
			PlayerController.Instance.AddItemCount(itemName, count);
			allTexts.Add(string.Format("You've got x{0} {1}", count, itemName));
			dm.SetText(allTexts);
			dm.Show();
			opened = true;
			if (DungeonManager.Instance!=null)
			{
				DungeonManager.Instance.chestState[DungeonManager.Instance.currentRoomId] = true;	
			}
			
		}
	}
}
