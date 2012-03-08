using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public float cameraSpeed;	
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.position.x > -230)
		{
			Vector3 delta = new Vector3(cameraSpeed * Time.deltaTime, 0.0f, 0.0f);
			transform.Translate(delta);
		}
	}
	
}
