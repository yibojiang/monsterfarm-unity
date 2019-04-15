using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterFarm
{
	public enum UIInputType
	{
		Up,
		Down,
		Left,
		Right,
		Confirm,
		Cancel,
		NextPage,
		LastPage,
		Interact,
	}
	public class UIController : MonoBehaviour
	{	
		private static UIController _instance;
		private Action<UIInputType> _inputAction;
		public static UIController Instance
		{
			get
			{
				if (!_instance) {
					_instance = (UIController) FindObjectOfType(typeof(UIController));
					if (!_instance)
					{
						GameObject go = new GameObject();
						_instance = go.AddComponent<UIController>();
						go.name = "UIController";
					}
				}
				
				
				return _instance;	
			}
		}

		private Stack<List<UIPanel>> _panelStack = new Stack<List<UIPanel>>();
		public Image uiFade;
		
		// Use this for initialization
		void Awake ()
		{
			var canvas = GameObject.FindObjectOfType<Canvas>();
			if (canvas != null)
			{
				var uiFadePreafab = Resources.Load("Prefab/UI/ui_fade");
				GameObject uiFadeObj = (GameObject)Instantiate(uiFadePreafab, canvas.transform);
				uiFade = uiFadeObj.GetComponent<Image>();
				uiFade.gameObject.SetActive(false);
			}
		}
		
		public void FadeOut(float duration, Action callback)
		{
			FadeTo(new Color(0, 0, 0, 0), Color.black, duration, callback);
		}
		
		public void FadeIn(float duration, Action callback)
		{
			FadeTo(Color.black,new Color(0, 0, 0, 0), duration, callback);
		}
		
		public void FadeOutIn(float duration, Action fadeOutCallback ,Action fadeInCallback)
		{
			FadeOut(duration / 2, () =>
			{
				if (fadeOutCallback != null)
				{
					fadeOutCallback();
				}

				FadeIn(duration / 2, () =>
				{
					if (fadeInCallback != null)
					{
						fadeInCallback();
					}
					uiFade.gameObject.SetActive(false);
				});
			});
		}

	public void FadeTo(Color from, Color to, float duration, Action callback)
		{
			StartCoroutine(FadeCo(from, to, duration, callback));
		}

		private IEnumerator FadeCo(Color from, Color to, float duration, Action callback)
		{
			uiFade.gameObject.SetActive(true);
			float timer = 0f;
			while (timer < duration)
			{
				uiFade.color = Color.Lerp(from, to,timer/duration);
				timer += Time.unscaledDeltaTime;
				yield return null;	
			}

			if (callback != null)
			{
				callback();
			}

			uiFade.color = to;
		}

		public void UpdatePlayerHP()
		{
			
		}
		

		public List<UIPanel> GetCurrentPanel()
		{
			if (_panelStack.Count > 0)
			{
				return _panelStack.Peek();
			}
			else
			{
				return null;
			}
		}
	
		public void PushPanel(UIPanel panel)
		{
			var newPanels = new List<UIPanel>();
			_panelStack.Push(newPanels);
			AddPanel(panel);
			
			_inputAction = null;
			_inputAction += panel.InputCallback;
		}

		public void AddPanel(UIPanel panel)
		{
			if (_panelStack.Count == 0)
			{
				var newPanels = new List<UIPanel>();
				_panelStack.Push(newPanels);
			}

			var topPanels = GetCurrentPanel();
			topPanels.Add(panel);
			panel.ShowPanel();
			
			_inputAction += panel.InputCallback;
		}

		public void RemovePanel(UIPanel panel)
		{
			var topPanels = GetCurrentPanel();
			topPanels.Remove(panel);
			
			_inputAction -= panel.InputCallback;
			if (topPanels.Count == 0)
			{
				PopPanel();
			}
		}

		public void PopPanel()
		{
			_panelStack.Pop();

			_inputAction = null;
			var topPanels = GetCurrentPanel();

			if (topPanels != null)
			{
				foreach (var p in topPanels)
				{
					_inputAction += p.InputCallback;
				}	
			}
			else
			{
				PlayerController.Instance.SetInGameState(InGameState.Play);
			}
		}

		public void InputHandle(UIInputType inputType)
		{
			if (_inputAction != null)
			{
				_inputAction(inputType);
			}
		}
		
	}

}
