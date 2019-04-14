using System;
using System.Collections;
using System.Collections.Generic;
using MonsterFarm;
using UnityEngine;
using Random = UnityEngine.Random;

public class Portal : MonoBehaviour {

	public Transform targetAnchor;
	public Vector3 offset = new Vector3(0, -0.3f, 0);
	public bool toOutDoor;
	public bool changeBgm = false;
	public AudioClip bgmClip;
	public Action enterCallback;

	void OnTriggerEnter2D(Collider2D col) {
		// Debug.Log(col.gameObject.name);
		if (col.CompareTag("Player")) {
			var playerPawn = col.GetComponent<PlayerPawn>();
			var body = col.gameObject.GetComponent<Rigidbody2D>();
			UIController.Instance.FadeOutIn(0.5f,() =>
			{
				if (enterCallback != null)
				{
					enterCallback();
				}
				body.position = targetAnchor.position + offset;
				for (int i = 0; i < playerPawn.Followers.Count; i++)
				{
					var follower = playerPawn.Followers[i]; 
					float rad = Random.Range(0f, 2f * Mathf.PI);
					float len = Random.Range(0.2f, 0.5f);
					playerPawn.Followers[i].transform.position = targetAnchor.position + offset + len * new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
					if (toOutDoor)
					{
						follower.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
					}
					else
					{
						follower.transform.localScale = new Vector3(1f, 1f, 1f);
					}
				}
			
				if (toOutDoor) {
					playerPawn.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
				}
				else {
					playerPawn.transform.localScale = new Vector3(1f, 1f, 1f);
				}

				if (changeBgm)
				{
					AudioManager.Instance.PlayBGM(bgmClip);
				}

				
			}, null
			);
			
		}
	}
}
