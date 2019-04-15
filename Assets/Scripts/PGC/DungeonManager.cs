using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour
{
	private static DungeonManager _instance;

	public static DungeonManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<DungeonManager>();	
			}

			return _instance;
		} 
	}
	public int width = 30;

	public int height = 30;

	public Tilemap groundMap;
	public Tilemap wallMap;

	public Tile[] groundTiles;
	public Tile[] wallTiles;
	
	Dictionary<int, Room> rooms = new Dictionary<int, Room>();
	public int currentRoomId = 0;
	public Portal[] portals;
	public Transform[] spawnPoints;
	public List<GameObject> monsters = new List<GameObject>();
	public List<GameObject> chests = new List<GameObject>();
	public Dictionary<int, bool> chestState = new Dictionary<int, bool>();
	
	// Use this for initialization
	void Start ()
	{
		int numOfRooms = 5;
		
		rooms = new Dictionary<int, Room>();
		DungeonGenerator.GenerateBFSRoom(rooms, numOfRooms);
//		for (int i = 0; i < rooms.Count; i++)
//		{
//			Debug.Log("id: " + rooms[i].id);
//			for (int j = 0; j < 4; j++)
//			{
//				Debug.Log((ConnectDirection)j + ": " + rooms[i].connections[j]);
//			}
//
//			Debug.Log("-----------------------");
//		}
		GenerateRoom();

	}

	public Room GetCurrentRoom()
	{
		return rooms[currentRoomId];
	}

	public void GenerateMonsters()
	{
		for (int i = 0; i < monsters.Count; i++)
		{
			Destroy(monsters[i]);
		}
		
		monsters.Clear();
		
		var room = GetCurrentRoom();
		for (int i = 0; i < room.monsters.Length; i++)
		{
			var monsterName = room.monsters[i];			
			var prefab = Resources.Load<GameObject>(string.Format("Prefab/Monster/{0}", monsterName));
			var monsterObj = (GameObject)Instantiate(prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
			monsters.Add(monsterObj);
		}
	}

	public void GenerateChests()
	{
		for (int i = 0; i < chests.Count; i++)
		{
			Destroy(chests[i]);
		}
		
		chests.Clear();
		
		var room = GetCurrentRoom();
		if (room.hasChest)
		{
			var prefab = Resources.Load("Prefab/Dungeon/chest");
			var chestObj = (GameObject)Instantiate(prefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
			var chest = chestObj.GetComponent<InteractChest>(); 
			chest.itemName = room.chestItem.itemName;
			chest.GetComponent<InteractChest>().count = room.chestItem.count;
			chests.Add(chestObj);

			if (!chestState.ContainsKey(room.id))
			{
				chestState[room.id] = false;
			}
			else
			{
				if (chestState[room.id])
				{
					chest.opened = true;	
				}
			}
		}
		
	}

	public void GenerateRoom()
	{
		GenerateMonsters();
		GenerateChests();
		groundMap.ClearAllTiles();
		wallMap.ClearAllTiles();
		
		var room = GetCurrentRoom();
		int roomWidth = room.width;
		int roomHeight = room.height;
		int hallWayWidth = 4;
		int hallWayLength = 2;


		for (int i = 0; i < roomWidth+hallWayLength*2; i++)
		{
			for (int j = 0; j < roomHeight+hallWayLength*2; j++)
			{
				wallMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), wallTiles[0]);
			}
		}

		portals[0].transform.position = new Vector3(hallWayLength+roomWidth+hallWayLength, hallWayLength+roomHeight/2);
		portals[0].GetComponent<BoxCollider2D>().size= new Vector2(0.4f,hallWayWidth); 
		portals[1].transform.position = new Vector3((hallWayLength + roomWidth + hallWayLength)/2, 0 );
		portals[1].GetComponent<BoxCollider2D>().size= new Vector2(hallWayWidth, 0.4f);
		portals[2].transform.position = new Vector3(0, hallWayLength+roomHeight/2);
		portals[2].GetComponent<BoxCollider2D>().size= new Vector2( 0.4f, hallWayWidth);
		portals[3].transform.position = new Vector3((hallWayLength + roomWidth + hallWayLength)/2, hallWayLength + roomHeight + hallWayLength );
		portals[3].GetComponent<BoxCollider2D>().size= new Vector2(hallWayWidth, 0.4f);
		for (int i = hallWayLength; i < hallWayLength + roomWidth; i++)
		{
			for (int j = hallWayLength; j < hallWayLength + roomHeight; j++)
			{
				groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[room.groundTileIdx]);
				wallMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), null);
			}
			
			//wallMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, hallWayLength + roomHeight, 0)), wallTiles[0]);
		}

		if (GetCurrentRoom().Up != -1)
		{
			for (int i = hallWayLength + roomWidth/2 - hallWayWidth/2; i < hallWayLength + roomWidth/2 + hallWayWidth/2; i++)
			{
				for (int j = hallWayLength + roomHeight; j < hallWayLength + roomHeight + hallWayLength; j++)
				{
					groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[room.groundTileIdx]);
					wallMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), null);
				}
			}
			portals[3].gameObject.SetActive(true);
			portals[3].enterCallback = () =>
			{
				currentRoomId = GetCurrentRoom().Up; 
				GenerateRoom();
			};
		}
		else
		{
			portals[3].gameObject.SetActive(false);
		}
		
		if (GetCurrentRoom().Down != -1)
		{
			for (int i = hallWayLength + roomWidth/2 - hallWayWidth/2; i < hallWayLength + roomWidth/2 + hallWayWidth/2; i++)
			{
				for (int j = 0; j < hallWayLength; j++)
				{
					groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[room.groundTileIdx]);
					wallMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), null);
				}
			}
			
			portals[1].changeMap = false;
			portals[1].gameObject.SetActive(true);
			
			if (room.Down == -2)
			{
				portals[1].changeMap = true;
				portals[1].loadScene = "level1";
				portals[1].unloadScene = "dungeon";
				portals[1].anchorObjName = "anchor_dungeon_entrance";
				portals[1].enterCallback = null;
			}
			else
			{
				portals[1].enterCallback = () =>
				{
					currentRoomId = GetCurrentRoom().Down; 
					GenerateRoom();
				};	
			}
		}
		else
		{
			portals[1].gameObject.SetActive(false);
		}
		
		if (GetCurrentRoom().Left != -1)
		{
			for (int i = 0; i < hallWayLength; i++)
			{
				for (int j = hallWayLength + roomHeight/2 - hallWayWidth/2; j < hallWayLength + roomHeight/2 + hallWayWidth/2; j++)
				{
					groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[room.groundTileIdx]);
					wallMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), null);
				}
			}
			portals[2].gameObject.SetActive(true);
			portals[2].enterCallback = () =>
			{
				currentRoomId = GetCurrentRoom().Left; 
				GenerateRoom();
			};
		}
		else
		{
			portals[2].gameObject.SetActive(false);
		}
		
		
		if (GetCurrentRoom().Right != -1)
		{
			for (int i = hallWayLength + roomWidth; i < hallWayLength + roomWidth + hallWayLength; i++)
			{
				for (int j = hallWayLength + roomHeight/2 - hallWayWidth/2; j < hallWayLength + roomHeight/2 + hallWayWidth/2; j++)
				{
					groundMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), groundTiles[0]);
					wallMap.SetTile(Vector3Int.CeilToInt(new Vector3(i, j, 0)), null);
				}
			}
			portals[0].gameObject.SetActive(true);
			portals[0].enterCallback = () =>
			{
				currentRoomId = GetCurrentRoom().Right; 
				GenerateRoom();
			};
		}
		else
		{
			portals[0].gameObject.SetActive(false);
		}
	}

}
