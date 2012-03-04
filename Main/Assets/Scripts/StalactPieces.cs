using UnityEngine;
using System.Collections;

public class StalactPieces : MonoBehaviour {
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
//		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) {
//			body.AddForce(10, 10, 10);
//		}
	}
	
	public void Fall() {
		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) {
			body.useGravity = true;
		}
	}
}
