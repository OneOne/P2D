using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float MaxMoveSpeed = 10.0f;
	public float JumpForce = 700.0f;
	public Transform GroundCheck;
	public LayerMask GroundLayer;

	public float GroundRaduis = 0.2f;


	// Use this for initialization
	void Start () {
		_rigidBody2D = GetComponent<Rigidbody2D> ();
	}
	
	private bool _facingRight = true;
	private bool _onGround =  false;

	private Rigidbody2D _rigidBody2D;

	void Update() {
		if (_onGround && Input.GetButtonDown ("Jump")) {
			_onGround = false;
			_rigidBody2D.AddForce (new Vector2 (0, JumpForce));
		}
	}

	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");

		_onGround = Physics2D.OverlapCircle (GroundCheck.position, GroundRaduis, GroundLayer);

				
		_rigidBody2D.velocity = new Vector2 (move * MaxMoveSpeed, _rigidBody2D.velocity.y);


		if (move > 0 && !_facingRight)
			Flip ();
		else if (move < 0 && _facingRight)
			Flip ();
	}

	void Flip() {
		_facingRight = !_facingRight;
		Vector3 lScale = transform.localScale;
		lScale.x *= -1;
		transform.localScale = lScale;
		//transform.localScale.x *= -1;
	}
}
