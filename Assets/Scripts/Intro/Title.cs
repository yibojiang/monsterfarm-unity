﻿using System.Collections;
using System.Collections.Generic;
using MonsterFarm;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
		{
			UIController.Instance.FadeOut(1f, () =>
			{
				SceneManager.LoadScene("intro", LoadSceneMode.Single);	
			});
		}
	}
}