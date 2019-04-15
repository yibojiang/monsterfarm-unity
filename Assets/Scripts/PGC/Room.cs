using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStruct
{
	public string itemName;
	public int count;

	public ItemStruct(string itemName, int count)
	{
		this.itemName = itemName;
		this.count = count;
	}
}

public class Room
{
	public int id;
	public string roomPrefab;
	public string[] monsters;
	public int width = 6;
	public int height = 6;

	public bool hasChest;
	public ItemStruct chestItem;

	public int Up
	{
		set { connections[(int) ConnectDirection.Up] = value; }
		get { return connections[(int) ConnectDirection.Up]; }
	}
	
	public int Left
	{
		set { connections[(int) ConnectDirection.Left] = value; }
		get { return connections[(int) ConnectDirection.Left]; }
	}
	
	public int Right
	{
		set { connections[(int) ConnectDirection.Right] = value; }
		get { return connections[(int) ConnectDirection.Right]; }
	}
	
	public int Down
	{
		set { connections[(int) ConnectDirection.Down] = value; }
		get { return connections[(int) ConnectDirection.Down]; }
	}
	
	// -1 is block, others number is the id of room
	public List<int> connections = new List<int>();

	public Room(int id)
	{
		this.id = id;
		connections.Add(-1);
		connections.Add(-1);
		connections.Add(-1);
		connections.Add(-1);
		string[] monsterPrefab = new[] {"mon_frank","mon_insect", "mon_dragon", "mon_trap"};
		int monsterCount = Random.Range(0, 3);
		monsters = new string[monsterCount];
		for (int i = 0; i < monsterCount; i++)
		{
			monsters[i] = monsterPrefab[Random.Range(0, monsterPrefab.Length)];
		}

		if (monsterCount == 0)
		{
			hasChest = true;
		}
		
		ItemStruct[] allItems = new []{new ItemStruct("apple", 2), new ItemStruct("pill", 1), new ItemStruct("coins", 200) };
		chestItem = allItems[Random.Range(0, allItems.Length)]; 
		

		width = Random.Range(6, 10);
		height = Random.Range(6, 10);
	}
}
