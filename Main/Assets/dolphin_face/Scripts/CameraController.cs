using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public float CAMERA_RATE = 0.5f;
	public float CAMERA_LIMIT = 45.0f;
	
	public bool FREEZE = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.CurrentGameState == GameState.EGameState.GameState_Active
		    && transform.position.y >= -CAMERA_LIMIT
		    && !FREEZE)
		{
			UpdateCameraInActiveState();
		}

	}
	
	void UpdateCameraInActiveState ()
	{
		// Move down on Y, that is all...
		var downVec = new Vector3(0.0f, -CAMERA_RATE, 0.0f);
		downVec = downVec * Time.deltaTime;
		
		downVec.y = Mathf.Max(downVec.y, -CAMERA_LIMIT);
		
		transform.Translate(downVec);
	}
}
