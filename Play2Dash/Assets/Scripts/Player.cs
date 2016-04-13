using UnityEngine;
using System.Collections;

public class playerHit : GameEvent{
	public int health;
	public playerHit(int thisHealth){
		health = thisHealth;
	}
}

public class Player : MonoBehaviour {
	
	public float MoveSpeed = 800.0f;
	public float JumpSpeed = 1000.0f;
	public float MinJumpSpeed = 200.0f;
	/*public float LongJumpTime = 0.3f;
	public float LongJumpSpeed = 16f;*/

	/*public Transform FrontCorner1;
	public Transform FrontCorner2;
	public Transform BottomCorner1;
	public Transform BottomCorner2;*/
	//public LayerMask GroundLayer;
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


	private bool _facingRight = false;
	private bool _bottomTouched =  false;
	private bool _frontTouched =  false;

	private bool _firstJump =  false;
	private bool _firstJumpEnd =  false;
	private bool _secondJump =  false;
	/*private bool _longJump =  false;
	private float _jumpTime = 0.0f;*/
	private bool _grinding = false;

	private Rigidbody2D _rigidBody2D;



	private BoxCollider2D _bottom_box;
	private CircleCollider2D _bottom_left;
	private CircleCollider2D _bottom_right;
	private BoxCollider2D _left_box;
	private BoxCollider2D _right_box;

	// Use this for initialization
	void Start () {
		_anim = GetComponentInChildren<Animator>();
		_rigidBody2D = GetComponent<Rigidbody2D> ();
		wallsMask = LayerMask.GetMask("Walls");

		_bottom_box = GetComponents<BoxCollider2D> () [1];
		_bottom_left = GetComponents<CircleCollider2D> () [0];
		_bottom_right = GetComponents<CircleCollider2D> () [1];
		_left_box = GetComponents<BoxCollider2D> () [2];
		_right_box = GetComponents<BoxCollider2D> () [3];
	}



	private bool _wasJumpDown = false;

	void Update() {
		bool isJumpDown = false;
		bool isJumpUp = false;

		if (Input.touchCount > 0) {
			Touch t = Input.touches [0];
			if (t.position.x > Screen.width / 3 && t.position.x < 2*Screen.width / 3 && t.position.y < Screen.height / 3) {
				isJumpDown = true;
				_wasJumpDown = true;
			}
		} else if (_wasJumpDown) {
			_wasJumpDown = false;
			isJumpUp = true;
		} else {
			isJumpDown = Input.GetButtonDown ("Jump");
			isJumpUp = Input.GetButtonUp ("Jump");
		}
			

		if (isJumpDown) {
			if (!_firstJump && (_bottomTouched || _grinding)) {
				_firstJump = true;
				_firstJumpEnd = false;

				// jump whil grinding
				if (_grinding) {
					if(_facingRight)
						_rigidBody2D.velocity = new Vector2 (MoveSpeed, JumpSpeed);
					else
						_rigidBody2D.velocity = new Vector2 (-MoveSpeed, JumpSpeed);
				}
				// classic jump
				else
					_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, JumpSpeed);
			}
			else if(_firstJumpEnd && !_secondJump) {
				_secondJump = true;
				// second jump
				_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, JumpSpeed);
			}

		}
		else if(isJumpUp) {
			_firstJump = false;
			_firstJumpEnd = true;

			if(_rigidBody2D.velocity.y > MinJumpSpeed)
				_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, MinJumpSpeed);
		}
	}

	void FixedUpdate () {
		float move = 0.0f;

		if(Input.touchCount > 0) {
			Touch t = Input.touches [0];
			if (t.position.x >= 0 && t.position.x < Screen.width / 3 && t.position.y < Screen.height / 3) {
				move = -1.0f;
			} else if (t.position.x > 2*Screen.width / 3 && t.position.y < Screen.height / 3) {
				move = 1.0f;
			}
		}
		else {
			Input.GetAxis ("Horizontal");
		}

		detectWalls();
		moveDeb = Mathf.Abs(move);

		// Evalute states
		_bottomTouched = _bottom_box.IsTouchingLayers (wallsMask) || _bottom_left.IsTouchingLayers (wallsMask) || _bottom_right.IsTouchingLayers (wallsMask);
		_frontTouched = /*_left_box.IsTouchingLayers (wallsMask) ||*/ _right_box.IsTouchingLayers (wallsMask) || _bottom_right.IsTouchingLayers (wallsMask);
		_grinding = /*Mathf.Abs(move) > 0f &&*/ _frontTouched && !_bottomTouched;

		if(_bottomTouched) {
			_firstJump = false;
			_secondJump = false;
		}

		//Animation part
		_anim.SetFloat("velocityY", _rigidBody2D.velocity.y);		
		if(move != 0)
			_anim.SetBool(runHash, true);
		else
			_anim.SetBool(runHash, false);

		if (_bottomTouched){
			_anim.SetBool("grounded",true);
			_anim.SetBool("grind", false);
		}

		_anim.SetBool("grind",_grinding);




		// classic move
		if(Mathf.Abs(move) > 0 && !_grinding)
			_rigidBody2D.velocity = new Vector2 (move * MoveSpeed, _rigidBody2D.velocity.y);
		
		// grind
		if (_grinding) {
			// move while grinding
			if( (move>0 && _facingRight) || (move<0 && !_facingRight) ) {
				_rigidBody2D.velocity = new Vector2 (move * MoveSpeed, _rigidBody2D.velocity.y);
			}
			// fall
			_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, _rigidBody2D.velocity.y / 1.1f);
		}


		if(_rigidBody2D.velocity.x > 0  && _facingRight)
			Flip ();				
		else if (_rigidBody2D.velocity.x  < 0 && !_facingRight)
			Flip ();
	}

	void Flip() {
		_facingRight = !_facingRight;
		Vector3 lScale = transform.localScale;
		lScale.x *= -1;
		transform.localScale = lScale;
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
