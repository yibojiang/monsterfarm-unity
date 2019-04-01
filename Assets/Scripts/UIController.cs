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
				}
				return _instance;	
			}
		}

		private Stack<List<UIPanel>> _panelStack = new Stack<List<UIPanel>>();
		public Image uiFade;

		
		// Use this for initialization
		void Start ()
		{
			
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
	
		// Update is called once per frame
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
