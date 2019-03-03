using UnityEngine;
using UnityEngine.UI;

namespace MonsterFarm
{
    public class InventoryPanel : UIPanel
    {
        private int _selIdx;
        public GridLayoutGroup gridLayout;
        public Image uiCursor;
        private Image[] iconList;
        private Vector2 _gridSize;

        private void Awake()
        {
            iconList = gridLayout.GetComponentsInChildren<Image>();
            _gridSize = gridLayout.cellSize;

        }
        public override void InputCallback(UIInputType uiInputType)
        {
            base.InputCallback(uiInputType);
            if (uiInputType == UIInputType.Left)
            {
                _selIdx--;
            }
            
            if (uiInputType == UIInputType.Right)
            {
                _selIdx++;
            }

            if (uiInputType == UIInputType.Up)
            {
                if (_selIdx - 3 >= 0)
                {
                    _selIdx -= 3;    
                }
            }

            if (uiInputType == UIInputType.Down)
            {
                if (_selIdx + 3 < iconList.Length)
                {
                    _selIdx += 3;    
                }
            }

            _selIdx = Mathf.Clamp(_selIdx, 0, iconList.Length - 1);
            uiCursor.rectTransform.position = iconList[_selIdx].rectTransform.position;
        }

        private void Update()
        {
            float lerpValue = Mathf.PingPong(Time.time  * 4, 1f);
            uiCursor.rectTransform.sizeDelta = Vector2.Lerp(
                new Vector2(_gridSize.x, _gridSize.y), 
                1.1f * new Vector2(_gridSize.x, _gridSize.y),
                lerpValue
            );
        }
    }
}