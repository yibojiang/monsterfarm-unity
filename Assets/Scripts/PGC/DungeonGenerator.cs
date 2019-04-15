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
public class DungeonGenerator
{

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

	public static ConnectDirection GetCounterDireaction(ConnectDirection direction)
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
	
	public static Room GenerateRoom(int fromRoomId, int roomId, ConnectDirection direction, Dictionary<int, Room> rooms, int numOfRooms)
	{
		Room room = new Room(roomId);
		room.connections[(int)GetCounterDireaction(direction)] = fromRoomId;
		ConnectDirection counterDirection = GetCounterDireaction(direction);
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
			if (rooms.Count >= numOfRooms)
			{
				break;
			}

			var generatedRoom = GenerateRoom(roomId, roomId+i+1, dirs[i], rooms, numOfRooms);
			room.connections[(int)dirs[i]] = generatedRoom.id;
		}

		rooms[room.id] = room;
		return room;
	}

	public static void GenerateBFSRoom(Dictionary<int, Room> rooms, int numOfRooms)
	{
		Queue<Room> pendingRooms = new Queue<Room>();
		Queue<ConnectDirection> pendingDirection = new Queue<ConnectDirection>();
		var entranceRoom = new Room(0);
		entranceRoom.Down = -2;
		pendingRooms.Enqueue(entranceRoom);
		pendingDirection.Enqueue(ConnectDirection.Up);
		rooms[0] = entranceRoom;
		int currentLength = 1;
		while (pendingRooms.Count > 0)
		{
			int popCounter = currentLength;
			currentLength = 0;
			while (popCounter > 0)
			{
				Room room = pendingRooms.Dequeue();

				ConnectDirection enterDir = pendingDirection.Dequeue();
				ConnectDirection counterDirection = GetCounterDireaction(enterDir);
				
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
					if (rooms.Count >= numOfRooms)
					{
						break;
					}

					Room connectedRoom = new Room(rooms.Count);
					rooms[connectedRoom.id] = connectedRoom;
					connectedRoom.connections[(int) GetCounterDireaction(dirs[i])] = room.id;
					room.connections[(int) dirs[i]] = connectedRoom.id; 
					pendingRooms.Enqueue(connectedRoom);
					pendingDirection.Enqueue(dirs[i]);
					currentLength++;
				}

				popCounter--;
			}
		}
	}
}
