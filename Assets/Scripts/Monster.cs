using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
	public AudioClip hitClip;
	public void GetHit (Vector3 pos) {
		var hurtParticlePrefab = Resources.Load("Prefab/Effect_BloodHit");
		var hurtPS = (GameObject)GameObject.Instantiate(hurtParticlePrefab, pos, Quaternion.identity);
		var am = AudioManager.Instance;
		am.PlaySFX(hitClip);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
