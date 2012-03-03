using System;
using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour
{
    public float moveSpeed = 0.1f;

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
		var direction = coyote.transform.position - transform.position;
		direction.Normalize();
		
		transform.position += direction * moveSpeed;
    }
}
