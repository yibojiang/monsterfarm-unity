using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public bool followTarget;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (followTarget)
			transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10f);
	}
}
