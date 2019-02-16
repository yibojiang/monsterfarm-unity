using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
	private static DialogManager instance_;
	public static DialogManager Instance {
		get {
			if (!instance_) {
				instance_ = (DialogManager) FindObjectOfType(typeof(DialogManager));
			}

			return instance_;
		}
	}

	public Text textDialog;
	public GameObject dialog;
	public List<string> allTexts;
	public int textIdx = 0;
	public System.Action finishAction;

	public void SetText(List<string> text, System.Action finishCallback) {
		allTexts = text;
		textIdx = 0;
		finishAction = finishCallback;
	}

	public void SetText(List<string> text) {
		SetText(text, null);
	}

	public void Show() {
		if (textIdx < allTexts.Count) {
			dialog.SetActive(true);
			textDialog.text = allTexts[textIdx];
			var pc = PlayerController.Instance;
			pc.playerPawn.isTalking = true;
		}
	}

	public void Next() {
		textIdx += 1;
		if (textIdx >= allTexts.Count) {
			Hide();
		}
		else {
			textDialog.text = allTexts[textIdx];
		}
	}

	public void Hide() {
		dialog.SetActive(false);
		var pc = PlayerController.Instance;
		pc.playerPawn.isTalking = false;
		if (finishAction != null) {
			Debug.Log("finishAction");
			finishAction();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
