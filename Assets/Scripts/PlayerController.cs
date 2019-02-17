using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    enum GameMode {
        Paused,
        InGame
    }

    enum InGameState {
        Play,
        UI
    }
    
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
    private GameMode _gameMode = GameMode.InGame;
    private InGameState _ingameState = InGameState.Play;
    private Camera _cam;
    public Image uiFade;
    public Image uiBook;
    
    public int maxInvertory = 7;
    public int inventoryIdx = 0;
    public Dictionary<string, int> items = new Dictionary<string, int>();
    private int _coins;
    public Text textCoins;
    public Text textInteract;
    public GameObject curBlueprint;
    
    [SerializeField]
    public List<InventoryItem> inventoryList;
    
    [System.Serializable]
    public class InventoryItem {
        public string itemName;
        public string prefabPath;
        public int count;

        public InventoryItem(string itemName, string prefabPath, int count)
        {
            itemName = itemName;
            prefabPath = prefabPath;
            count = count;
        }
    }

    private void Awake()
    {
        _cam = Camera.main;
        inventoryList = new List<InventoryItem>(
            new InventoryItem[]
            {
                new InventoryItem("statue_archer", "Prefab/Statue_Archer", 2),
                new InventoryItem("tree", "Prefab/Tree", 0),
                new InventoryItem("woodvat", "Prefab/Woodvat", 0),
                new InventoryItem("statue_archer", "Prefab/Statue_Archer", 2),
                new InventoryItem("statue_archer", "Prefab/Statue_Archer", 2),
                new InventoryItem("statue_archer", "Prefab/Statue_Archer", 2),
                new InventoryItem("statue_archer", "Prefab/Statue_Archer", 2),
            });
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;
        if (curBlueprint) {
            var pos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _cam.nearClipPlane));
            curBlueprint.transform.position = pos - new Vector3(0f, 0.5f, 0f);
            if (Input.GetMouseButton(0)) {
                var bp = curBlueprint.GetComponent<BuildBlueprint>();
                bp.EnableCollider();
                Destroy(bp);
                curBlueprint = null;
            }
        }
        
        // Show Inventory
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (_ingameState == InGameState.Play)
            {
                _ingameState = InGameState.UI;
                uiFade.gameObject.SetActive(true);
                uiBook.gameObject.SetActive(true);
            }
            else if (_ingameState == InGameState.UI)
            {
                _ingameState = InGameState.Play;
                uiFade.gameObject.SetActive(false);
                uiBook.gameObject.SetActive(false);
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

                    var item = inventoryList[inventoryIdx];

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
    
    public void AddCoins(int coins) {
        _coins += coins;
        textCoins.text = string.Format("X {0}", _coins.ToString("N0"));
    }
}
