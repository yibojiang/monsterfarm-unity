using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room
{
	public int id;
	public string roomPrefab;
	
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
