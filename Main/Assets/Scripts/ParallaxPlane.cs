using UnityEngine;
using System.Collections;

public class ParallaxPlane : MonoBehaviour {
	
	public float depth;
	public Camera camera;
	
	private float cameraXBase;
	private float[] uBase;
	
	void Start() {
		cameraXBase = camera.transform.position.x;
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		uBase = new float[mesh.uv.Length];
		for (int i = 0; i < mesh.uv.Length; i++) {
			uBase[i] = mesh.uv[i].x;
		}
	}
	
	void Update () {
		float uDelta = computeUDelta ();
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector2[] uvs = new Vector2[mesh.uv.Length];
		for (int i = 0; i < mesh.uv.Length; i++) {
			uvs[i] = new Vector2(uBase[i] + uDelta, mesh.uv[i].y);
		}
		mesh.uv = uvs;
	}
	
	float computeUDelta() {
		float cameraXDelta = (camera.transform.position.x - cameraXBase);
		float nonzeroDepth = (depth == 0.0f) ? 0.01f : depth;
		float uDelta = -(1.0f / nonzeroDepth) * cameraXDelta;
		return uDelta;
	}
}
