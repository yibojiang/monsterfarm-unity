using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class UpdateDrawOrder : MonoBehaviour {
	private SpriteRenderer sprite_;
	public int offset = 0;
	
	[MenuItem("Tools/DestroyAllDrawOrder")]
	static void DoSomething()
	{
		var drawOrders = GameObject.FindObjectsOfType<UpdateDrawOrder>();
		foreach (var drawCom in drawOrders)
		{
			DestroyImmediate(drawCom);
		}
	}
	// Use this for initialization
	void Start () {
		sprite_ = this.gameObject.GetComponent<SpriteRenderer>();
	}
	
	void Reset () {
		sprite_ = this.gameObject.GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		sprite_.sortingOrder = (int)((100-transform.position.y) * 10) + offset;
	}
}
