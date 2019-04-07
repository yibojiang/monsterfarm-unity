using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using JetBrains.Annotations;
using MonsterFarm;
using UnityEngine.Serialization;

public enum GameMode {
    Paused,
    Play
}

public enum InGameState {
    Play,
    UI
}

public enum WeaponType
{
    Sword,
    Bow
}

public class Weapon
{
    public WeaponType weaponType { get; }

    public Weapon(WeaponType weaponType)
    {
        this.weaponType = weaponType;
    }
}

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance {
        get {
            if (!_instance) {
                _instance = (PlayerController) FindObjectOfType(typeof(PlayerController));
            }
            return _instance;
        }
    }

    public PlayerPawn playerPawn;
    private Vector3 _playerInput;
    private InGameState _ingameState = InGameState.Play;
    private Camera _cam;
    public Image uiFade;
    public UIPanel inventoryPanel;
    
    public int maxInvertory = 7;
    public int inventoryIdx = 0;
    
    public int Coins { get; set; }
    public Text textCoins;
    public Text textInteract;
    public GameObject curBlueprint;
    public Image uiHp;
    public Image uiMaxHp;
    
    public List<BlueprintItem> blueprintList;
    public List<Weapon> weapons = new List<Weapon>();
    public int weaponIdx;

    [System.Serializable]
    public class BlueprintItem {
        public string itemName;
        public string prefabPath;
        public int count;

        public BlueprintItem(string itemName, string prefabPath, int count)
        {
            this.itemName = itemName;
            this.prefabPath = prefabPath;
            this.count = count;
        }
    }

    public class InventoryUIItem
    {
        public string itemName;

        public int ItemCount
        {
            get { return PlayerController.Instance.items[itemName]; }
        }

        [CanBeNull] public Sprite sprite;

        public InventoryUIItem()
        {
            
        }
        
        public InventoryUIItem(string itemName)
        {
            this.itemName = itemName;
            var tex = Resources.Load<Texture2D>($"UI/InventoryItemIcon/ui_{itemName}");
            sprite = Sprite.Create(tex, new Rect(0f,0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }
    
    public Dictionary<string, int> items = new Dictionary<string, int>();
    private Dictionary<string, Sprite> _itemSprite = new Dictionary<string, Sprite>();
    private Dictionary<string, InventoryUIItem> _inventoryUIItemDict = new Dictionary<string, InventoryUIItem>();

    public InventoryUIItem[] GetInventoryItems(int size, int offset)
    {
        InventoryUIItem[] tmpItems = new InventoryUIItem[size];
        int idx = 0;
        foreach (var item in items)
        {
            if (item.Value > 0 && _inventoryUIItemDict.ContainsKey(item.Key))
            {
                tmpItems[idx] = _inventoryUIItemDict[item.Key];
                idx++;
            }

            if (idx >= size)
            {
                break;
            }
        }

        return tmpItems;
    }

    public bool HasItem(string itemName, int itemCount)
    {
        return (items.ContainsKey(itemName) && items[itemName] - itemCount >= 0);
    }

    public int UseItem(string itemName)
    {
        Debug.Log($"Use Item {itemName}");
        if (items.ContainsKey(itemName) && items[itemName] > 0)
        {
            items[itemName]--;
            // TODO: Use Apple
            return items[itemName];
        }

        return 0;
    }
    
    public void AddItemCount(string itemName, int itemCount)
    {
        items[itemName] += itemCount;
    }

    public void LoseItemCount(string itemName, int itemCount)
    {
        items[itemName] -= itemCount;
    }

    private void Awake()
    {
        _cam = Camera.main;
        blueprintList = new List<BlueprintItem>
        {
            new BlueprintItem("statue_archer", "Prefab/statue_archer", 2),
            new BlueprintItem("tree", "Prefab/tree", 0),
            new BlueprintItem("woodvat", "Prefab/woodvat", 0),
            new BlueprintItem("statue_archer", "Prefab/statue_archer", 2),
            new BlueprintItem("statue_archer", "Prefab/statue_archer", 2),
            new BlueprintItem("statue_archer", "Prefab/statue_archer", 2),
            new BlueprintItem("statue_archer", "Prefab/statue_archer", 2),
        };
        
        _inventoryUIItemDict.Add("apple", new InventoryUIItem("apple"));
    }

    private void Start()
    {
        weapons.Add(new Weapon(WeaponType.Bow));
        weapons.Add(new Weapon(WeaponType.Sword));
    }

    public void UpdatePlayerUI(int hp, int maxHp)
    {
        var tmpSize = uiMaxHp.rectTransform.sizeDelta;
        tmpSize.x = hp * 20;
        uiHp.rectTransform.sizeDelta = tmpSize;
        tmpSize.x = maxHp * 20;
        uiMaxHp.rectTransform.sizeDelta = tmpSize;
    }
    

    public void SetInGameState(InGameState inGameState)
    {
        _ingameState = inGameState;
    }

    public WeaponType GetCurrentWeaponType()
    {
        return weapons[weaponIdx].weaponType;
    }

    public void SwitchWeapon()
    {
        weaponIdx++;
        if (weaponIdx >= weapons.Count)
        {
            weaponIdx = 0;
        }
    }
    

    private void Update()
    {
        if (!playerPawn.alive)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (_ingameState == InGameState.Play)
            {
                _ingameState = InGameState.UI;
                UIController.Instance.PushPanel(inventoryPanel);
            }
        }
        
        if (_ingameState == InGameState.Play)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchWeapon();    
            }

            if (GetCurrentWeaponType() == WeaponType.Bow)
            {
                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftShift)) {
                    playerPawn.AimStart();                

                    if (Input.GetMouseButtonDown(0)) {
                        playerPawn.Shoot();
                    }
                }
                else
                {
                    playerPawn.CancelAmining();
                }    
            }
            else if (GetCurrentWeaponType() == WeaponType.Sword)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    playerPawn.MeleeAttack();
                }    
            }

            

            var mousePos = Input.mousePosition;
            if (curBlueprint) {
                var pos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _cam.nearClipPlane));
                curBlueprint.transform.position = pos - new Vector3(0f, 0.5f, 0f);
                if (Input.GetMouseButtonDown(0)) {
                    var bp = curBlueprint.GetComponent<BuildBlueprint>();
                    bp.EnableCollider();
                    Destroy(bp);
                    curBlueprint = null;
                }
            }
            
            _playerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            _playerInput = Vector3.Normalize(_playerInput);
            playerPawn.SetControlMovement(_playerInput);
            
            for (var i = 1; i <= 9; i++) {
                if (Input.GetKeyDown(i.ToString())) {
                    inventoryIdx = i - 1;
                    if (inventoryIdx < maxInvertory) {
                        if (curBlueprint != null) {
                            Destroy(curBlueprint);
                        }

                        var item = blueprintList[inventoryIdx];

                        if (item != null) {
                            try {
                                curBlueprint = Instantiate(Resources.Load(item.prefabPath, typeof(GameObject))) as GameObject;
                                var bp = curBlueprint.AddComponent<BuildBlueprint>();
                                bp.DisableCollider();
                            }
                            catch (System.Exception ex) {
                                Debug.Log(ex);
                            }	
                        }
                    }
                }
            }
        }
        else if (_ingameState == InGameState.UI)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                UIController.Instance.InputHandle(UIInputType.Up);
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                UIController.Instance.InputHandle(UIInputType.Down);
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                UIController.Instance.InputHandle(UIInputType.Left);
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                UIController.Instance.InputHandle(UIInputType.Right);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                UIController.Instance.InputHandle(UIInputType.LastPage);
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                UIController.Instance.InputHandle(UIInputType.NextPage);
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                UIController.Instance.InputHandle(UIInputType.Confirm);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIController.Instance.InputHandle(UIInputType.Cancel);
            }
        }
    }
    
    public void AddCoins(int coins) {
        Coins += coins;
        textCoins.text = string.Format("X {0}", Coins.ToString("N0"));
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }
    
}
