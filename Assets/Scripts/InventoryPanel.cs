using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MonsterFarm
{
    public class InventoryPanel : UIPanel
    {
        private int _selIdx;
        public GridLayoutGroup gridLayout;
        public Image iconCursor;
        public Image tabCursor;
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
                if (_selIdx - gridLayout.constraintCount >= 0)
                {
                    _selIdx -= gridLayout.constraintCount;    
                }
            }

            if (uiInputType == UIInputType.Down)
            {
                if (_selIdx + gridLayout.constraintCount < iconList.Length)
                {
                    _selIdx += gridLayout.constraintCount;
                }
            }

            _selIdx = Mathf.Clamp(_selIdx, 0, iconList.Length - 1);
            iconCursor.rectTransform.position = iconList[_selIdx].rectTransform.position;
        }

        private void Update()
        {
            float lerpValue = Mathf.PingPong(Time.time  * 4, 1f);
            iconCursor.rectTransform.sizeDelta = Vector2.Lerp(
                new Vector2(_gridSize.x, _gridSize.y), 
                1.1f * new Vector2(_gridSize.x, _gridSize.y),
                lerpValue
            );
        }
    }
}