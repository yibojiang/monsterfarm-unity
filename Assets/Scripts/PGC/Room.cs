using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room
{
	public int id;
	public string roomPrefab;

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
	}
}
