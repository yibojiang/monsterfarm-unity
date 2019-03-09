using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MonsterFarm
{
    public class InventoryPanel : UIPanel
    {
        private int _selIdx = 0;
        public GridLayoutGroup gridLayout;
        public GridLayoutGroup gridCounterLayout;
        public Image iconCursor;
        public Image tabCursor;
        private Image[] _iconList;
        private Text[] _counterTextList;
        private Vector2 _gridSize;
        public GridLayoutGroup tabLayout;
        private Image[] _tabList;

        private int _tabIdx = 0;
        private PlayerController.InventoryUIItem[] _uiItems;

        private void Awake()
        {
            _iconList = gridLayout.GetComponentsInChildren<Image>();
            _tabList = tabLayout.GetComponentsInChildren<Image>();
            _counterTextList = gridCounterLayout.GetComponentsInChildren<Text>();
            _gridSize = gridLayout.cellSize;
        }

        public override void ShowPanel()
        {
            base.ShowPanel();
            var pc = PlayerController.Instance;
            _uiItems = pc.GetInventoryItems(9, 0);
            for (int i = 0; i < _iconList.Length; i++)
            {
                if (_uiItems[i] != null && _uiItems[i].sprite != null)
                {
                    _iconList[i].sprite = _uiItems[i].sprite;
                    _counterTextList[i].text = _uiItems[i].ItemCount.ToString();
                    _iconList[i].color = Color.white;
                }
                else
                {
                    _iconList[i].color = Color.clear;
                    _counterTextList[i].text = "";
                }
                
            }
//            int idx = 0;
//            foreach (var item in pc.items)
//            {
//                Debug.Log($"itemName: {item.Key}, itemCount: {item.Value}");
//                idx++;
//            }
        }

        public override void InputCallback(UIInputType uiInputType)
        {
            base.InputCallback(uiInputType);
            if (uiInputType == UIInputType.Left)
            {
                _selIdx--;
            }
            else if (uiInputType == UIInputType.Right)
            {
                _selIdx++;
            }
            else if (uiInputType == UIInputType.Up)
            {
                if (_selIdx - gridLayout.constraintCount >= 0)
                {
                    _selIdx -= gridLayout.constraintCount;    
                }
            }
            else if (uiInputType == UIInputType.Down)
            {
                if (_selIdx + gridLayout.constraintCount < _iconList.Length)
                {
                    _selIdx += gridLayout.constraintCount;
                }
            }
            else if (uiInputType == UIInputType.LastPage)
            {
                _tabIdx--;
            }
            else if (uiInputType == UIInputType.NextPage)
            {
                _tabIdx++;
            }
            else if (uiInputType == UIInputType.Confirm)
            {
                if (_selIdx < _uiItems.Length && _uiItems[_selIdx] != null)
                {
                    var selItem = _uiItems[_selIdx].itemName;
                    if (selItem != null)
                    {
                        int count = PlayerController.Instance.UseItem(selItem);
                        if (count == 0)
                        {
                            _iconList[_selIdx].color = Color.clear;
                            _counterTextList[_selIdx].text = "";
                        }
                        else
                        {
                            _counterTextList[_selIdx].text = count.ToString();    
                        }    
                    }
                }
            }
            else if (uiInputType == UIInputType.Cancel)
            {
//                Debug.Log("cancel");
            }

            _selIdx = Mathf.Clamp(_selIdx, 0, _iconList.Length - 1);
            _tabIdx = Mathf.Clamp(_tabIdx, 0, _tabList.Length - 1);
            tabCursor.rectTransform.position = _tabList[_tabIdx].rectTransform.position;
            iconCursor.rectTransform.position = _iconList[_selIdx].rectTransform.position;
        }
    }
}