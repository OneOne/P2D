using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	public Vector3 moveVector;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(moveVector * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("test");
	}
}
