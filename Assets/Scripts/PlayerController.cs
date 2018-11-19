using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
 [System.Serializable]
public class InventoryItem {
	public string itemName;
	public string prefabPath;
	public int count;
}

public class PlayerController : MonoBehaviour {

	private static PlayerController instance_;
	public static PlayerController Instance {
		get {
			if (!instance_) {
				instance_ = (PlayerController) FindObjectOfType(typeof(PlayerController));
			}
			return instance_;
		}
	}
	
	public SpriteRenderer sprite_;
	public float maxMovingSpeed = 1.0f;
	private Vector3 movingVel;
	private Animator animSM_;
	// private bool faceingRight_ = false;
	public float acc = 3f;
	public float drag = 10f;

	private Vector3 lastMovingVel_;
	private bool inDoor;

	[SerializeField]
	public List<InventoryItem> inventoryList;

	public int maxInvertory = 7;
	public int inventoryIdx = 0;
	public GameObject curBlueprint;

	public bool buildingMode;

	public Interact interactTarget;
	public Text textInteract;

	public bool isTalkeing = false;

	private Camera cam;

	private int coins_;
	private bool isAiming_ = false;

	public Transform shootTarget;
	private GameObject arrow_;

	public int Coins {
		get {
			return coins_;
		}
		set {
			coins_ = value;
		}
	}
	public Text textCoins;
	public Dictionary<string, int> items = new Dictionary<string, int>();

	// Use this for initialization
	void Start () {
		// sprite_ = this.gameObject.GetComponent<SpriteRenderer>();
		animSM_ = sprite_.gameObject.GetComponent<Animator>();
		cam = Camera.main;
		Debug.Log(cam);
	}

	public void AddCoins(int coins) {
		coins_ += coins;
		textCoins.text = string.Format("X {0}", coins_.ToString("N0"));
	}

	void Update() {
		for (int i = 1; i <= 9; i++) {
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

		var mousePos = Input.mousePosition;
		if (curBlueprint) {
			var pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
			curBlueprint.transform.position = pos - new Vector3(0f, 0.5f, 0f);
			if (Input.GetMouseButton(0)) {
				var bp = curBlueprint.GetComponent<BuildBlueprint>();
				bp.EnableCollider();
				Destroy(bp);
				curBlueprint = null;
			}
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			if (interactTarget != null && !isTalkeing) {
				interactTarget.InteractAction();
			}
			else {
				var dm = DialogManager.Instance;
				dm.Next();
			}
		}

		if (Input.GetMouseButton(1)) {
			if (!isAiming_) {
				AimStart();
			}
			isAiming_ = true;

			if (Input.GetMouseButtonUp(0)) {
				animSM_.SetTrigger("Shoot");
				Shoot();
			}
		}
		else {
			if (arrow_) {
				Destroy(arrow_);
			}
			isAiming_ = false;
		}

		animSM_.SetBool("IsAiming", isAiming_);

		if (interactTarget!= null) {
			textInteract.gameObject.SetActive(true);
		}
		else {
			textInteract.gameObject.SetActive(false);
		}
		
		// Debug.Log("idx: " + inventoryIdx.ToString());
		// When currect select item is not null
		// if (inventoryList[inventoryIdx] != null) {

		// }
	}

	void AimStart() {
		var arrowPrefab = Resources.Load("Prefab/Arrow");
		arrow_ = (GameObject)GameObject.Instantiate(arrowPrefab, shootTarget.position, Quaternion.identity);
		arrow_.transform.parent = shootTarget;
		arrow_.transform.localRotation = Quaternion.identity;
	}
	
	void Shoot() {
		arrow_.transform.parent = null;
		var mousePos = Input.mousePosition;
		var pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
		var shootingDir = pos - shootTarget.position;
		arrow_.GetComponent<Arrow>().Shoot(shootingDir);
		arrow_ = null;
		isAiming_ = false;
		// var arrowPrefab = Resources.Load("Prefab/Arrow");
		// var arrow = (GameObject)GameObject.Instantiate(arrowPrefab, shootTarget.position, Quaternion.identity);
		// arrow.transform.parent = shootTarget;
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (!isAiming_) {
			var playerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
			playerInput = Vector3.Normalize(playerInput);

			movingVel += playerInput * acc;

			if (movingVel.magnitude > 0.1) {
				movingVel -= movingVel * drag;
				movingVel = Vector3.ClampMagnitude(movingVel, maxMovingSpeed);
				lastMovingVel_ = movingVel;
			}
			else {
				movingVel = Vector3.zero;
			}
			// Debug.Log(movingVel);

			transform.position = transform.position + movingVel * Time.fixedDeltaTime;
			animSM_.SetFloat("Speed", movingVel.magnitude);
			// var direction = (Mathf.Atan2(movingVel.y, movingVel.x));
			animSM_.SetFloat("DirectionX", lastMovingVel_.x);
			animSM_.SetFloat("DirectionY", lastMovingVel_.y);
		}
		else {
			var mousePos = Input.mousePosition;
			var pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
			var shootingDir = pos - shootTarget.position;
			Vector3.Normalize(shootingDir);
				// curBlueprint.transform.position = pos - new Vector3(0f, 0f, 0f);
				// if (Input.GetMouseButton(0)) {
				// 	var bp = curBlueprint.GetComponent<BuildBlueprint>();
				// 	bp.EnableCollider();
				// 	Destroy(bp);
				// 	curBlueprint = null;
				// }
			// }
			animSM_.SetFloat("ShootingDirectionX", shootingDir.x);
			animSM_.SetFloat("ShootingDirectionY", shootingDir.y);
			var eulerAngles = shootTarget.transform.localEulerAngles;
			eulerAngles.z = Mathf.Atan2(shootingDir.y, shootingDir.x) * Mathf.Rad2Deg;
			shootTarget.transform.localEulerAngles = eulerAngles;
		}
		
		// Debug.Log(direction);
		// Debug.Log(movingVel.magnitude);
		// if (movingVel.x > 0)
		// 	sprite_.flipX = true;
		// else if (movingVel.x < 0)
		// 	sprite_.flipX = false;

		// if (faceingRight_)
		// if (movingVel.x)
		// sprite_.flipX = faceingRight_;
		// movingSpeed += 
		// Debug.Log(Input.GetAxis("Horizontal"));
		// Debug.Log(Input.GetAxis("Vertical"));
	}
}
