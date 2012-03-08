using UnityEngine;
using System.Collections;

public class TestCameraHandler : MonoBehaviour 
{
	public Transform parent;
	Vector3 offset;
	
	// Use this for initialization
	void Start () {
		
		offset = transform.position - parent.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = parent.position + offset;
	}
	
}
