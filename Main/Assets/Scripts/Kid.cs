using System;
using UnityEngine;
using System.Collections;

public class Kid : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float targetDistance = 0.1f;

    private AnimationHandler animHandler;
	private Coyote coyote;

	// Use this for initialization
	void Start ()
	{
	    animHandler = GetComponent<AnimationHandler>();
		coyote = GameObject.FindWithTag("coyote").GetComponent<Coyote>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		var deltaTarget = coyote.transform.position - transform.position;
		var distance = deltaTarget.magnitude;
		
		var direction = deltaTarget / distance;
		direction.Normalize();
		
		if (distance > targetDistance)
		{
			transform.position += direction * moveSpeed;
		}
    }
}
