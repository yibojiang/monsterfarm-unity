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
		int roomWidth = 6;
		int roomHeight = 6;
		int hallWayWidth = 2;
		int hallWayLength = 2;
		groundMap.ClearAllTiles();
		for (int i = hallWayLength; i < hallWayLength + roomWidth; i++)
		{
			for (int j = hallWayLength; j < hallWayLength + roomHeight; j++)
			{
				groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[0]);		
			}
			
			//wallMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, hallWayLength + roomHeight, 0)), wallTiles[0]);
		}

		if (GetCurrentRoom().Up != -1)
		{
			for (int i = hallWayLength + roomWidth/2 - hallWayWidth/2; i < hallWayLength + roomWidth/2 + hallWayWidth/2; i++)
			{
				for (int j = hallWayLength + roomHeight; j < hallWayLength + roomHeight + hallWayLength; j++)
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
			for (int i = hallWayLength + roomWidth/2 - hallWayWidth/2; i < hallWayLength + roomWidth/2 + hallWayWidth/2; i++)
			{
				for (int j = 0; j < hallWayLength; j++)
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
			for (int i = 0; i < hallWayLength; i++)
			{
				for (int j = hallWayLength + roomHeight/2 - hallWayWidth/2; j < hallWayLength + roomHeight/2 + hallWayWidth/2; j++)
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
			for (int i = hallWayLength + roomWidth; i < hallWayLength + roomWidth + hallWayLength; i++)
			{
				for (int j = hallWayLength + roomHeight/2 - hallWayWidth/2; j < hallWayLength + roomHeight/2 + hallWayWidth/2; j++)
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
