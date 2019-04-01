using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStart : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		SceneManager.LoadScene("player", LoadSceneMode.Additive);
	}

	void Start() {
		PlayerController.Instance.playerPawn.transform.position = transform.position;
	}
}
