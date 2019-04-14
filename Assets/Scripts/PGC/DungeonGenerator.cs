using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ConnectDirection {
	Right = 0,
	Down = 1,
	Left = 2,
	Up = 3,
	Entrance = 4,
}
public class DungeonGenerator : MonoBehaviour
{

	public int width = 30;

	public int height = 30;
	// Use this for initialization
	void Start ()
	{
		int numOfRooms = 5;
		int counter = 0;
		List<Room> rooms = new List<Room>();
		GenerateRoom(-1, 0, ConnectDirection.Left, rooms, ref counter, numOfRooms);
		for (int i = 0; i < rooms.Count; i++)
		{
			Debug.Log("id: " + rooms[i].id);
			for (int j = 0; j < 4; j++)
			{
				Debug.Log((ConnectDirection)j + ": " + rooms[i].connections[j]);
			}

			Debug.Log("-----------------------");
		}
	}

	public static void Shuffle<T>(IList<T> list)
	{
		RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
		int n = list.Count;
		while (n > 1)
		{
			byte[] box = new byte[1];
			do provider.GetBytes(box);
			while (!(box[0] < n * (Byte.MaxValue / n)));
			int k = (box[0] % n);
			n--;
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public ConnectDirection getCounterDireaction(ConnectDirection direction)
	{
		if (direction == ConnectDirection.Up)
		{
			return ConnectDirection.Down;
		}
		else if (direction == ConnectDirection.Down)
		{
			return ConnectDirection.Up;
		}
		else if (direction == ConnectDirection.Left)
		{
			return ConnectDirection.Right;
		}
		else
		{
			return ConnectDirection.Left;
		}
	}
	
	public Room GenerateRoom(int fromRoomId, int roomId, ConnectDirection direction, List<Room> rooms, ref int counter, int numOfRooms)
	{
		Room room = new Room(roomId);
		counter++;
		room.connections[(int)getCounterDireaction(direction)] = fromRoomId;
		ConnectDirection counterDirection = getCounterDireaction(direction);
		var dirs = new List<ConnectDirection>();
		for (int i = 0; i < 4; i++)
		{
			if (i != (int)counterDirection)
			{
				dirs.Add((ConnectDirection)i);		
			}
		}
		
		Shuffle(dirs);

		int maxConnection = Random.Range(1, 3);
		for (int i = 0; i < maxConnection; i++)
		{
			if (counter > numOfRooms)
			{
				break;
			}

			var generatedRoom = GenerateRoom(roomId, roomId+i+1, dirs[i], rooms, ref counter, numOfRooms);
			room.connections[(int)dirs[i]] = generatedRoom.id;
		}

		rooms.Add(room);
		return room;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
