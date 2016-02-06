﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float MaxMoveSpeed = 10.0f;
	public float JumpForce = 700.0f;
	public Transform BottomSide;
	public Transform FrontCorner1;
	public Transform FrontCorner2;
	public Transform BottomCorner1;
	public Transform BottomCorner2;
	public LayerMask GroundLayer;

	public float TouchDetectionRadius = 0.2f;


	// Use this for initialization
	void Start () {
		_rigidBody2D = GetComponent<Rigidbody2D> ();
	}
	
	private bool _facingRight = false;
	private bool _bottomTouched =  false;
	private bool _leftTouched =  false;
	private bool _firstJump =  false;
	private bool _secondJump =  false;

	private Rigidbody2D _rigidBody2D;

	void Update() {
		if (Input.GetButtonDown ("Jump")) {
			if (_bottomTouched) {
				_bottomTouched = false;
				_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, 0);
				_rigidBody2D.AddForce (new Vector2 (0, JumpForce));
			} else if (_firstJump && !_secondJump) {
				_secondJump = true;
				_rigidBody2D.velocity = new Vector2 (_rigidBody2D.velocity.x, 0);
				_rigidBody2D.AddForce (new Vector2 (0, JumpForce));
			}
		}
		if (Input.GetButtonUp ("Jump")) {
			_firstJump = true;
		}
	}

	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");

		_bottomTouched 	= Physics2D.OverlapArea (	
			new Vector2 (BottomCorner1.transform.position.x, BottomCorner1.transform.position.y), 
			new Vector2 (BottomCorner2.transform.position.x, BottomCorner2.transform.position.y),
			GroundLayer);
		_leftTouched 	= Physics2D.OverlapArea (	
			new Vector2 (FrontCorner1.transform.position.x, FrontCorner1.transform.position.y), 
			new Vector2 (FrontCorner2.transform.position.x, FrontCorner2.transform.position.y),
			GroundLayer);

		if (_bottomTouched)
			_secondJump = false;


		if (move > 0 && !_facingRight)
			Flip ();				
		else if (move < 0 && _facingRight)
			Flip ();

		if( (move > 0 && !_leftTouched) || (move < 0 && !_leftTouched) )
			_rigidBody2D.velocity = new Vector2 (move * MaxMoveSpeed, _rigidBody2D.velocity.y);
	
	}
	void Flip() {
		_facingRight = !_facingRight;
		Vector3 lScale = transform.localScale;
		lScale.x *= -1;
		transform.localScale = lScale;
		//transform.localScale.x *= -1;
	}
}
