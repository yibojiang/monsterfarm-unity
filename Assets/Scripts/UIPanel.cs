using UnityEngine;

namespace MonsterFarm
{
    public class UIPanel : MonoBehaviour
    {
        public void ShowPanel()
        {
            this.gameObject.SetActive(true);
        }
    
        public void HidePanel()
        {
            this.gameObject.SetActive(false);
        }

        public virtual void InputCallback(UIInputType uiInputType)
        {
            if (uiInputType == UIInputType.Cancel)
            {
                HidePanel();
                UIController.Instance.RemovePanel(this);
            }
        }
    }
}
