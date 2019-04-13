using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pathfinding;

public class GameTools {
	
	[MenuItem("LevelTools/AddPathGraph")]
	private static void AddPathGrid()
	{
//		var astarPath = GameObject.FindObjectOfType<AstarPath>();
//		int len = astarPath.data.graphs.Length;
//		var graph = astarPath.data.graphs[len - 1];
		AstarData data = AstarPath.active.data;

		// This creates a Grid Graph
		GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;
		
		// Setup a grid graph with some values
		int width = 20;
		int depth = 20;
		float nodeSize = 1;
		
		gg.center = new Vector3(50, 50, 0);
		
		// Updates internal size from the above values
		gg.SetDimensions(width, depth, nodeSize);
		gg.collision.use2D = true;
		gg.collision.diameter = 0.6f;
		gg.collision.type = ColliderType.Sphere;
		gg.collision.mask = 1 << LayerMask.NameToLayer("Wall");
		
		gg.neighbours = NumNeighbours.Four;
		gg.rotation = new Vector3(gg.rotation.y - 90, 270, 90);
		
		// Scans all graphs
		AstarPath.active.Scan();
		

//		astarPath.data.AddGraph()
//		ArrayUtility.Add(astarPath.data);	
//		astarPath.data.graphs[len] 
//		astarPath.data.
	}
}
