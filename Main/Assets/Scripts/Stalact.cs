using UnityEngine;
using System.Collections;

public class Stalact : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnCollisionEnter(Collision other)
	{
		// Only deal damage to the first thing hit. If ground is hit first, then the kid is not hurt.
		//Destroy (gameObject.transform.parent.gameObject);
	}
}
