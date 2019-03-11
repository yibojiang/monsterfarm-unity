using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPawn : MobPawn {
	
	public SpriteRenderer _sprite;
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
	public Vector2 controlMovement;

	private Camera _cam;

	public bool IsInvincible { get; set;  } = false;

	protected void Awake()
	{
		base.Awake();
		_animSm = _sprite.gameObject.GetComponent<Animator>();
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

	

	public void SetControlMovement(Vector2 dir)
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

		_rigidBody.MovePosition(_rigidBody.position + _movingVel * Time.fixedDeltaTime);
		_animSm.SetFloat("Speed", _movingVel.magnitude);
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

	public override void Hurt(Vector2 pos, int damage)
	{
		base.Hurt(pos, damage);
		StartCoroutine(RecoverActionCo());
		StartCoroutine(HitBackCo(pos));
	}

	IEnumerator HitBackCo(Vector2 pos)
	{
		float timer = 0f;
		float hitBackDuration = 0.1f;
		Vector2 hitDir = _rigidBody.position - pos;
		hitDir.Normalize();
		
		while (timer < hitBackDuration)
		{
			timer += Time.fixedDeltaTime;
			//_rigidBody.MovePosition(_rigidBody.position + hitDir * 5f * Time.fixedDeltaTime);
			_rigidBody.position = _rigidBody.position + hitDir * 5f * Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator RecoverActionCo()
	{
		float timer = 0f;
		float invincibleDuration = 1f;
		IsInvincible = true;
		var color = _sprite.color;
		while (timer < invincibleDuration)
		{
			timer += Time.deltaTime;
			color.a = Mathf.PingPong(timer * 15, 1);
			_sprite.color = color;
			yield return new WaitForEndOfFrame();
		}

		color.a = 1f;
		_sprite.color = color;
		IsInvincible = false;
	}

	public override void Die()
	{
		PlayerController.Instance.GameOver();
	}
	
//	void OnCollisionEnter2D (Collision2D col) {
//		if (!IsInvincible)
//		{
//			if (col.gameObject.CompareTag("Monster")) {
//				var monster = col.gameObject.GetComponent<MonsterPawn>();
//				var contact = col.GetContact(0);
//				Hurt(col.rigidbody.position, monster.HitDamage);
//				var hitDir = _rigidBody.position - contact.rigidbody.position;
//				//_rigidBody.MovePosition(_rigidBody.position - hitDir.normalized * 100);
//				//_rigidBody.position = _rigidBody.position - hitDir.normalized * .5f;
////				_rigidBody.AddForce(hitDir * 50000);
//			}	
//		}
//	}

	private void OnCollisionStay2D(Collision2D other)
	{
		if (!IsInvincible)
		{
			if (other.gameObject.CompareTag("Monster")) {
				var monster = other.gameObject.GetComponent<MonsterPawn>();
				var contact = other.GetContact(0);
				Hurt(other.rigidbody.position, monster.HitDamage);
				var hitDir = _rigidBody.position - contact.rigidbody.position;
				//_rigidBody.MovePosition(_rigidBody.position - hitDir.normalized * 100);
				//_rigidBody.position = _rigidBody.position - hitDir.normalized * .5f;
//				_rigidBody.AddForce(hitDir * 50000);
			}	
		}
	}
}
