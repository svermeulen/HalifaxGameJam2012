using UnityEngine;
using System.Collections;

public class BigBadGuy : MonoBehaviour 
{
	public float moveRangeX = 1;
	public float moveRangeY = 1;
	
	public float moveSpeedX = 0.1f;
	public float moveSpeedY = 0.1f;
	
	public float frequencyX = 1;
	public float frequencyY = 1;
	
	float thetaX = 0;
	float thetaY = 0;
	
	Vector3 startPos;
	
	void Start()
	{
		startPos = transform.position;
	}
	
	void Update () 
	{
		float offsetX = Mathf.Sin(frequencyX * thetaX);
		float offsetY = Mathf.Cos(frequencyY * thetaY);
		
		transform.position = startPos + Vector3.left * offsetX * moveRangeX + Vector3.up * offsetY * moveRangeY;
		
		thetaX += moveSpeedX * Time.deltaTime;
		thetaY += moveSpeedY * Time.deltaTime;
	}
}
