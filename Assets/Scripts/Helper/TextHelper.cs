using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHelper : MonoBehaviour
{
	public Color targetColor = Color.gray;
	void Start()
	{	
		StartCoroutine(LerpColor());
	}
	// Use this for initialization
	IEnumerator LerpColor () {
		
		Text text = GetComponent<Text>();
		var srcColor = text.color;
		while (true)
		{
			var lerpValue = ((int) Time.time % 2) == 0 ? Time.time % 1 : 1f - Time.time % 1;
			text.color = Color.Lerp(srcColor, targetColor, lerpValue);
			yield return null;
		}
	}
}
