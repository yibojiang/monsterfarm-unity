using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFeed : Interact
{
	private MonsterPawn mob;

	public GameObject happyEmojiPrefab;
	// Use this for initialization
	void Awake ()
	{
		mob = GetComponentInParent<MonsterPawn>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override bool CanInteract()
	{
		return PlayerController.Instance.HasItem(mob.favouriteItem, 1);
	}

	public override void InteractAction()
	{
		if (mob.canFeed)
		{
			var pc = PlayerController.Instance;
			if (pc.HasItem(mob.favouriteItem, 1))
			{
				pc.LoseItem(mob.favouriteItem, 1);
				mob.AddFriendShip(1);
				var emoji = Instantiate(happyEmojiPrefab, mob.transform.position + new Vector3(0f, 0.2f, 0f), Quaternion.identity);
			}
		}
	}
}
