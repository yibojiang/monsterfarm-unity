using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour {
	public int width = 30;

	public int height = 30;

	public Tilemap groundMap;
	public Tilemap wallMap;

	public Tile[] groundTiles;
	public Tile[] wallTiles;
	
	Dictionary<int, Room> rooms = new Dictionary<int, Room>();
	public int currentRoomId = 0;
	public Portal[] portals;
	
	// Use this for initialization
	void Start ()
	{
		int numOfRooms = 5;
		
		rooms = new Dictionary<int, Room>();
		DungeonGenerator.GenerateBFSRoom(rooms, numOfRooms);
		for (int i = 0; i < rooms.Count; i++)
		{
			Debug.Log("id: " + rooms[i].id);
			for (int j = 0; j < 4; j++)
			{
				Debug.Log((ConnectDirection)j + ": " + rooms[i].connections[j]);
			}

			Debug.Log("-----------------------");
		}

		GenerateTiles();

	}

	public Room GetCurrentRoom()
	{
		return rooms[currentRoomId];
	}

	public void GenerateTiles()
	{
		groundMap.ClearAllTiles();
		for (int i = 2; i < 8; i++)
		{
			for (int j = 2; j < 8; j++)
			{
				groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[0]);		
			}
		}

		if (GetCurrentRoom().Up != -1)
		{
			for (int i = 4; i < 6; i++)
			{
				for (int j = 7; j < 10; j++)
				{
					groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[0]);
				}
			}
			
			portals[3].enterCallback = () =>
			{
				currentRoomId = GetCurrentRoom().Up; 
				GenerateTiles();
			};
		}
		
		if (GetCurrentRoom().Down != -1)
		{
			for (int i = 4; i < 6; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[0]);
				}
			}
			
			portals[1].enterCallback = () =>
			{
				currentRoomId = GetCurrentRoom().Down; 
				GenerateTiles();
			};
		}
		
		if (GetCurrentRoom().Left != -1)
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 4; j < 6; j++)
				{
					groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[0]);
				}
			}
			
			portals[2].enterCallback = () =>
			{
				currentRoomId = GetCurrentRoom().Left; 
				GenerateTiles();
			};
		}
		
		if (GetCurrentRoom().Right != -1)
		{
			for (int i = 7; i < 10; i++)
			{
				for (int j = 4; j < 6; j++)
				{
					groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[0]);
				}
			}
			portals[0].enterCallback = () =>
			{
				currentRoomId = GetCurrentRoom().Right; 
				GenerateTiles();
			};
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
