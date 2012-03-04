using UnityEngine;
using System.Collections;

public class StalactPieces : MonoBehaviour {
	
	public float forceAmount = 5;
	public float minX = -1;
	public float maxX = 1;
		
	bool hasRun = false;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!hasRun)
		{
			foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) 
			{
	            body.AddForce(new Vector3(Random.Range(minX, maxX), forceAmount, 0));
	        }
			hasRun = true;
		}
	}
}
