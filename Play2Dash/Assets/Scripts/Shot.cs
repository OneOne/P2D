using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	public Vector3 moveVector;

	Animator anim;
	int collideHash = Animator.StringToHash("collide");

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(moveVector * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other) {
		anim.SetTrigger(collideHash);
	}

}
