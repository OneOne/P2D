using UnityEngine;
using System.Collections;

public class playerHit : GameEvent{
	public int health;
	public playerHit(int thisHealth){
		health = thisHealth;
	}
}

public class Player : MonoBehaviour {
	
	public float MoveSpeed = 10.0f;
	public float JumpSpeed = 60.0f;
	public float LongJumpTime = 0.3f;
	public float LongJumpSpeed = 16f;

	public Transform FrontCorner1;
	public Transform FrontCorner2;
	public Transform BottomCorner1;
	public Transform BottomCorner2;
	public LayerMask GroundLayer;
	LayerMask wallsMask;

	public float TouchDetectionRadius = 0.2f;

	Animator _anim;
	int runHash = Animator.StringToHash("run");
	int jumpHash = Animator.StringToHash("jump");
	int hitHash = Animator.StringToHash("hit");
	int deathHash = Animator.StringToHash("death");
	int grindHash = Animator.StringToHash("grind");
	int groundedHash = Animator.StringToHash("grounded");
	int health = 100;

	// Use this for initialization
	void Start () {
		_anim = GetComponentInChildren<Animator>();
		_rigidBody2D = GetComponent<Rigidbody2D> ();
		wallsMask = LayerMask.GetMask("Walls");
	}

	private bool _facingRight = false;
	public bool _bottomTouched =  false;
	private bool _leftTouched =  false;
	private bool _firstJump =  false;
	private bool _secondJump =  false;
	private bool _longJump =  false;
	private float _jumpTime = 0.0f;
	private bool _grinded = false;

	private Rigidbody2D _rigidBody2D;

	void Update() {

		if (Input.GetButtonDown ("Jump")) {
			_jumpTime += Time.deltaTime;

			if (_bottomTouched || _grinded == true) {
				_bottomTouched = false;
				_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, JumpSpeed);
			} else if (_firstJump && !_secondJump) {
				_secondJump = true;
				_longJump = false;
				_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, JumpSpeed);
			}
		} else if (Input.GetButton ("Jump")) {
			_jumpTime += Time.deltaTime;

			if (!_longJump && _jumpTime > LongJumpTime) {
				_longJump = true;
				_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, LongJumpSpeed);
			}
		}

		if (Input.GetButtonUp ("Jump")) {
			_firstJump = true;
			_longJump = false;
			_jumpTime = 0.0f;
			_anim.SetBool("grounded",false);
		}

	}

	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");

		detectWalls();
		moveDeb = Mathf.Abs(move);

		if(Mathf.Abs(move) > 0f && frontHitOn == true && _bottomTouched == false){
			_grinded = true;
			_anim.SetBool("grind",true);
			_firstJump = false;
			_longJump = false;
			_secondJump = false;
			_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, _rigidBody2D.velocity.y / 1.1f);
		}

		//Animation part	
		_anim.SetFloat("velocityY", _rigidBody2D.velocity.y);		
		if(move != 0)
			_anim.SetBool(runHash, true);
		else
			_anim.SetBool(runHash, false);

		_bottomTouched 	= Physics2D.OverlapArea (	
			new Vector2 (BottomCorner1.transform.position.x, BottomCorner1.transform.position.y), 
			new Vector2 (BottomCorner2.transform.position.x, BottomCorner2.transform.position.y),
			GroundLayer);
		_leftTouched 	= Physics2D.OverlapArea (	
			new Vector2 (FrontCorner1.transform.position.x, FrontCorner1.transform.position.y), 
			new Vector2 (FrontCorner2.transform.position.x, FrontCorner2.transform.position.y),
			GroundLayer);

		if (_bottomTouched){
			_secondJump = false;
			_anim.SetBool("grounded",true);
			_anim.SetBool("grind", false);
		}

		if(frontHitOn == false){
			_grinded = false;
			_anim.SetBool("grind", false);	
		}


		if (move > 0 && _facingRight)
			Flip ();				
		else if (move < 0 && !_facingRight)
			Flip ();

		if( ((move > 0 && frontHitOn == false) || (move < 0 && frontHitOn == false)) ){
			_rigidBody2D.velocity = new Vector2 (move * MoveSpeed, _rigidBody2D.velocity.y);
		}

	}

	void Flip() {
		_facingRight = !_facingRight;
		Vector3 lScale = transform.localScale;
		lScale.x *= -1;
		transform.localScale = lScale;
		//transform.localScale.x *= -1;
	}

	public void hit(int hitValue){
		health -= hitValue;
		playerHit _playerHitEvent = new playerHit(health);
		_anim.SetTrigger(hitHash);
		Events.instance.Raise(_playerHitEvent);
		if(health <= 0){
			_anim.SetTrigger(deathHash);
			this.enabled = false;
		}
	}


	public bool frontHitOn = false;
	public float moveDeb;

	void detectWalls(){
		Vector2 grindRayDir;
		if(_facingRight == false){
			grindRayDir = Vector2.right;
		}
		else{
			grindRayDir = -Vector2.right;
		}

		RaycastHit2D frontHit = Physics2D.Raycast(transform.position, grindRayDir, 100,wallsMask);
		Debug.DrawRay(transform.position, grindRayDir * 100, Color.green, 0, false);

		if (frontHit.collider != null) {
			frontHitOn = true;
		}
		else{
			frontHitOn = false;
		}
	}
		
}
