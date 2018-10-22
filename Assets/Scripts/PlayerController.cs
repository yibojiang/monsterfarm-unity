﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 [System.Serializable]
public class InventoryItem {
	public string itemName;
	public string prefabPath;
	public int count;
}

public class PlayerController : MonoBehaviour {
	
	public SpriteRenderer sprite_;
	public float maxMovingSpeed = 1.0f;
	private Vector3 movingVel;
	private Animator animSM_;
	// private bool faceingRight_ = false;
	public float acc = 3f;
	public float drag = 10f;

	private Vector3 lastMovingVel_;
	private bool inDoor;

	[SerializeField]
	public List<InventoryItem> inventoryList;

	public int maxInvertory = 7;
	public int inventoryIdx = 0;
	public GameObject curBlueprint;

	public bool buildingMode;

	private Camera cam;

	// Use this for initialization
	void Start () {
		// sprite_ = this.gameObject.GetComponent<SpriteRenderer>();
		animSM_ = sprite_.gameObject.GetComponent<Animator>();
		cam = Camera.main;
		Debug.Log(cam);
	}

	void Update() {
		for (int i = 1; i <= 9; i++) {
			if (Input.GetKeyDown(i.ToString())) {
				inventoryIdx = i - 1;
				if (inventoryIdx < maxInvertory) {
					if (curBlueprint != null) {
						Destroy(curBlueprint);
					}

					var item = inventoryList[inventoryIdx];

					if (item != null) {
						try {
							curBlueprint = Instantiate(Resources.Load(item.prefabPath, typeof(GameObject))) as GameObject;
							var bp = curBlueprint.AddComponent<BuildBlueprint>();
							bp.DisableCollider();
						}
						catch (System.Exception ex) {
							Debug.Log(ex);
						}	
					}
				}
			}
		}

		var mousePos = Input.mousePosition;
		if (curBlueprint) {
			var pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
			curBlueprint.transform.position = pos - new Vector3(0f, 0.5f, 0f);
			if (Input.GetMouseButton(0)) {
				var bp = curBlueprint.GetComponent<BuildBlueprint>();
				bp.EnableCollider();
				Destroy(bp);
				curBlueprint = null;
			}
		}
		
		// Debug.Log("idx: " + inventoryIdx.ToString());
		// When currect select item is not null
		// if (inventoryList[inventoryIdx] != null) {

		// }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var playerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
		// Debug.Log(playerInput);
		
		playerInput = Vector3.Normalize(playerInput);

		movingVel += playerInput * acc;

		if (movingVel.magnitude > 0.1) {
			movingVel -= movingVel * drag;
			movingVel = Vector3.ClampMagnitude(movingVel, maxMovingSpeed);
			lastMovingVel_ = movingVel;
		}
		else {
			movingVel = Vector3.zero;
		}
		// Debug.Log(movingVel);

		transform.position = transform.position + movingVel * Time.fixedDeltaTime;
		animSM_.SetFloat("Speed", movingVel.magnitude);
		// var direction = (Mathf.Atan2(movingVel.y, movingVel.x));
		
		
		animSM_.SetFloat("DirectionX", lastMovingVel_.x);
		animSM_.SetFloat("DirectionY", lastMovingVel_.y);
		// Debug.Log(direction);
		// Debug.Log(movingVel.magnitude);
		// if (movingVel.x > 0)
		// 	sprite_.flipX = true;
		// else if (movingVel.x < 0)
		// 	sprite_.flipX = false;

		// if (faceingRight_)
		// if (movingVel.x)
		// sprite_.flipX = faceingRight_;
		// movingSpeed += 
		// Debug.Log(Input.GetAxis("Horizontal"));
		// Debug.Log(Input.GetAxis("Vertical"));
	}
}
