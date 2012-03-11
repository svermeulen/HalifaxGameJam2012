using UnityEngine;
using System.Collections;

public class BigBadGuy : MonoBehaviour 
{
	public AudioClip soundClip;
	public float moveRangeX = 1;
	public float moveRangeY = 1;
	
	public float moveSpeedX = 0.1f;
	public float moveSpeedY = 0.1f;
	
	public float frequencyX = 1;
	public float frequencyY = 1;
	
	public float thetaX = 0;
	float thetaY = 0;
	GameObject camera;
	
	Vector3 startPos;
	GameObject kid;
	bool wasOnLeft = true;
	
	void Start()
	{
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		kid = GameObject.FindGameObjectWithTag("kid");
		startPos = transform.position;
	}
	
	void Update () 
	{
		bool isLeft = transform.position.x < kid.transform.position.x;
		
		if (isLeft != wasOnLeft)
		{
			camera.GetComponent<AudioSource>().PlayOneShot(soundClip);
		}
		
		wasOnLeft = isLeft;
		
		float offsetX = Mathf.Sin(frequencyX * thetaX);
		float offsetY = Mathf.Cos(frequencyY * thetaY);
		
		transform.position = startPos + Vector3.left * offsetX * moveRangeX + Vector3.up * offsetY * moveRangeY;
		
		thetaX += moveSpeedX * Time.deltaTime;
		thetaY += moveSpeedY * Time.deltaTime;
	}
}
