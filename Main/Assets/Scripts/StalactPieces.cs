using UnityEngine;
using System.Collections;

public class StalactPieces : MonoBehaviour {
	
	private float timerStart;
	
	// Use this for initialization
	void Start () 
	{
		timerStart = -1.0f;
	}
	
	// Update is called once per frame
	void Update () {
//		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) {
//			body.AddForce(10, 10, 10);
//		}
		if (timerStart > 0.0f && Time.time - timerStart > 1.0f) {
			Destroy (gameObject);
		}
	}
	
	public void Fall() {
		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) {
			body.useGravity = true;
			timerStart = Time.time;
		}
	}
}
