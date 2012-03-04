using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float targetDistance = 0.1f;

    private AnimationHandler animHandler;
	private Kid kid;

	// Use this for initialization
	void Start ()
	{
	    animHandler = GetComponent<AnimationHandler>();
		kid = GameObject.FindWithTag("kid").GetComponent<Kid>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		var deltaTarget = kid.transform.position - transform.position;
		var distance = deltaTarget.magnitude;
		
		var direction = deltaTarget / distance;
		direction.Normalize();
		
		if (distance > targetDistance)
		{
			transform.position += direction * moveSpeed;
		}
    }
}
