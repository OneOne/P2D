using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public GameObject ObjectToFollow;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pos = this.transform.position;
		pos.x = ObjectToFollow.transform.position.x;
		pos.y = ObjectToFollow.transform.position.y;
		this.transform.position = pos;
	}
}
