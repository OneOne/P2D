using UnityEngine;
using System.Collections;

public class playerAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void destroy(){
		GameObject.Destroy(transform.parent.gameObject);
	}
}
