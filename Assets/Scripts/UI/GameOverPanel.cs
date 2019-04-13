using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterFarm
{
    public class GameOverPanel : UIPanel
    {
        public override void InputCallback(UIInputType uiInputType)
        {
            Debug.Log(uiInputType);
            if (uiInputType == UIInputType.Confirm)
            {
                UIController.Instance.FadeTo(new Color(0,0,0, 0), Color.black, 2, () =>
                {
                    SceneManager.LoadScene("title");    
                });
            }
        }
    }
}