using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPawn : MobPawn {
	
	public SpriteRenderer sprite_;
	public float maxMovingSpeed = 1.0f;
	private Vector3 _movingVel;
	private Animator _animSm;

	public float acc = 3f;
	public float drag = 10f;

	private Vector3 _lastMovingVel;
	private bool _inDoor;

	private int _coins;
	private bool _isAiming = false;

	public Transform shootTarget;
	private GameObject _arrow;

	public AudioClip arrowSFX;

	public bool isTalking = false;
		
	
	public Vector3 controlMovement;

	
	private Camera _cam;

	private void Awake()
	{
		_animSm = sprite_.gameObject.GetComponent<Animator>();
		_cam = Camera.main;
	}

	public override void SetInteractTarget(Interact target)
	{
		if (target == null)
		{
			interactTarget = null;
			PlayerController.Instance.textInteract.gameObject.SetActive(false);
		}
		else
		{
			interactTarget = target;
			PlayerController.Instance.textInteract.text = target.interactMessage;
			PlayerController.Instance.textInteract.gameObject.SetActive(true);
		}
	}

	private void Update() {

		
		if (Input.GetKeyDown(KeyCode.E)) {
			if (interactTarget != null && !isTalking) {
				interactTarget.InteractAction();
			}
			else {
				var dm = DialogManager.Instance;
				dm.Next();
			}
		}

		if (Input.GetMouseButton(1)) {
			if (!_isAiming) {
				AimStart();
			}
			_isAiming = true;

			if (Input.GetMouseButtonUp(0)) {
				_animSm.SetTrigger("Shoot");
				Shoot();
			}
		}
		else {
			if (_arrow) {
				Destroy(_arrow);
			}
			_isAiming = false;
		}

		_animSm.SetBool("IsAiming", _isAiming);

		if (interactTarget!= null) {
			PlayerController.Instance.textInteract.gameObject.SetActive(true);
		}
		else {
			PlayerController.Instance.textInteract.gameObject.SetActive(false);
		}
		
		// Debug.Log("idx: " + inventoryIdx.ToString());
		// When currect select item is not null
		// if (inventoryList[inventoryIdx] != null) {

		// }
	}

	void AimStart() {
		var arrowPrefab = Resources.Load("Prefab/Arrow");
		_arrow = (GameObject)GameObject.Instantiate(arrowPrefab, shootTarget.position, Quaternion.identity);
		_arrow.transform.parent = shootTarget;
		_arrow.transform.localRotation = Quaternion.identity;
		_arrow.GetComponent<BoxCollider2D>().enabled = false;
	}
	
	void Shoot() {
		var am = AudioManager.Instance;
		am.PlaySFX(arrowSFX);
		var arrowSmokePrefab = Resources.Load("Prefab/Smoke_Arrow");
		var arrowSmoke_ = (GameObject)GameObject.Instantiate(arrowSmokePrefab, shootTarget.position, Quaternion.identity);
		arrowSmoke_.transform.eulerAngles = shootTarget.eulerAngles;

		_arrow.transform.parent = null;
		var mousePos = Input.mousePosition;
		var pos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _cam.nearClipPlane));
		var shootingDir = pos - shootTarget.position;
		_arrow.GetComponent<Arrow>().Shoot(shootingDir);
		_arrow.GetComponent<BoxCollider2D>().enabled = true;
		_arrow = null;
		_isAiming = false;

		// var arrowPrefab = Resources.Load("Prefab/Arrow");
		// var arrow = (GameObject)GameObject.Instantiate(arrowPrefab, shootTarget.position, Quaternion.identity);
		// arrow.transform.parent = shootTarget;
	}

	

	public void SetControlMovement(Vector3 dir)
	{
		controlMovement = dir.normalized * acc;
	}
	// Update is called once per frame
	void FixedUpdate () {
		_movingVel += controlMovement * Time.fixedDeltaTime;

		if (_movingVel.magnitude > 0.1) {
			_movingVel -= _movingVel * drag;
			_movingVel = Vector3.ClampMagnitude(_movingVel, maxMovingSpeed);
			_lastMovingVel = _movingVel;
		}
		else {
			_movingVel = Vector3.zero;
		}

		if (_isAiming) {
			_movingVel = Vector3.zero;
			shootTarget.gameObject.SetActive(true);
		}
		else {
			shootTarget.gameObject.SetActive(false);
		}

		transform.position = transform.position + _movingVel * Time.fixedDeltaTime;
		_animSm.SetFloat("Speed", _movingVel.magnitude);
		// var direction = (Mathf.Atan2(movingVel.y, movingVel.x));
		_animSm.SetFloat("DirectionX", _lastMovingVel.x);
		_animSm.SetFloat("DirectionY", _lastMovingVel.y);
		
		if (_isAiming) {
			shootTarget.gameObject.SetActive(true);
			var mousePos = Input.mousePosition;
			var pos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _cam.nearClipPlane));
			var shootingDir = pos - shootTarget.position;
			shootingDir.Normalize();
			Vector3.Normalize(shootingDir);

			_animSm.SetFloat("ShootingDirectionX", shootingDir.x);
			_animSm.SetFloat("ShootingDirectionY", shootingDir.y);
			var eulerAngles = shootTarget.transform.localEulerAngles;
			eulerAngles.z = Mathf.Atan2(shootingDir.y, shootingDir.x) * Mathf.Rad2Deg;
			shootTarget.transform.localEulerAngles = eulerAngles;
		}
	}
}
