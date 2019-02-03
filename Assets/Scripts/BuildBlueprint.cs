using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBlueprint : MonoBehaviour {
	private Collider2D col;
	private SpriteRenderer[] sprites;
	// Use this for initialization
	void Awake () {
		col = GetComponentInChildren<Collider2D>();
		sprites = GetComponentsInChildren<SpriteRenderer>();
	}

	public void EnableCollider() {
		col.enabled = true;
		for (int i = 0; i < sprites.Length; i++) {
			sprites[i].color = Color.white;
		}
	}

	public void DisableCollider() {
		for (int i = 0; i < sprites.Length; i++) {
			sprites[i].color = Color.green;
		}
		col.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
