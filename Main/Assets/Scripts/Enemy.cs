using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float targetDistance = 0.1f;

    private AnimationHandler animHandler;
	private Kid kid;

    enum State
    {
        Moving,
		Attacking
    }
	
	State state;
	
	// Use this for initialization
	void Start ()
	{
	    animHandler = GetComponent<AnimationHandler>();
		kid = GameObject.FindWithTag("kid").GetComponent<Kid>();
		
		state = State.Moving;
	}
		
	// Update is called once per frame
	void Update ()
    {
	    switch (state)
	    {
	        case State.Moving:
	            Moving();
                break;
			
	        case State.Attacking:
	            Attacking();
                break;

	        default:
	            throw new ArgumentOutOfRangeException();
	    }
	}
	
	void Moving()
	{
		var deltaTarget = kid.transform.position - transform.position;
		var distance = deltaTarget.magnitude;

		if (deltaTarget.x < 0)
		{
			animHandler.ChangeAnim("MoveLeft");
		}
		else
		{
			animHandler.ChangeAnim("MoveRight");
		}
		
		if (distance <= targetDistance)
		{
			//state = State.Attacking;
			//animHandler.ChangeAnim("AttackLeft");
			return;
		}
		
		var direction = deltaTarget;
		direction.y = 0;
		direction.Normalize();
		
		transform.position += direction * moveSpeed;
    }
	
	void Attacking()
	{
		
    }
}
