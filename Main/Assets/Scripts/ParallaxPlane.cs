using UnityEngine;
using System.Collections;

public class ParallaxPlane : MonoBehaviour {
	
	public float depth;
	public Camera camera;
	
	private float cameraXBase;
	
	void Start() {
		cameraXBase = camera.transform.position.x;
	}
	
	void Update () {
		float xOffset = computeXOffset ();
		Vector3 p = gameObject.transform.localPosition;
		gameObject.transform.localPosition = new Vector3(xOffset, p.y, p.z);
	}
	
	float computeXOffset() {
		float cameraXDelta = (camera.transform.position.x - cameraXBase);
		float fixedDepth = (depth < 0.1f) ? 0.1f : depth;
		return -cameraXDelta * (1.0f / fixedDepth - 1.0f);
	}
}
