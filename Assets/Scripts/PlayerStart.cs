using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStart : MonoBehaviour
{
	private bool teleport = false;
	// Use this for initialization
	void Awake () {
		if (!PlayerController.isLoaded)
		{
			SceneManager.LoadScene("player", LoadSceneMode.Additive);
			teleport = true;
		}
		
	}

	void Start()
	{
		if (teleport)
		{
			PlayerController.Instance.playerPawn.transform.position = transform.position;	
		}
	}

}
