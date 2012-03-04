using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float targetDistance = 0.1f;

    private AnimationHandler animHandler;
	private Kid kid;
	
	public float velocityDampening = 0.95f;
	public float maxSpeed;

    enum State
    {
        Moving,
		Attacking
    }
	
	State state;
	Vector3 velocity;
	
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
		CommonUpdate();
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
	
	void CommonUpdate()
	{
        //transform.position += velocity * Time.deltaTime;
	}
	
	void MovableCommon()
	{
		var deltaTarget = kid.transform.position - transform.position;
		var distance = deltaTarget.magnitude;
		
		var direction = deltaTarget / distance;

		velocity += direction * moveSpeed * Time.deltaTime;
		
		var speed = velocity.magnitude;
		
		if (speed > maxSpeed)
		{
			velocity = maxSpeed * velocity / speed;
		}
	}
	
	void Moving()
	{
		var deltaTarget = kid.transform.position - transform.position;
		var distance = deltaTarget.magnitude;
		
		var direction = deltaTarget / distance;
		
		var rigidBody = GetComponent<Rigidbody>();
		
		//rigidBody.AddForce(direction * moveSpeed);
		
		velocity += direction * moveSpeed * Time.deltaTime;
		
		var speed = velocity.magnitude;
		
		if (speed > maxSpeed)
		{
			velocity = maxSpeed * velocity / speed;
		}
		
		if (deltaTarget.x < 0)
		{
			animHandler.ChangeAnim("MoveLeft");
		}
		else
		{
			animHandler.ChangeAnim("MoveRight");
		}
		
		/*
		if (distance <= targetDistance)
		{
			velocity *= velocityDampening;
			state = State.Attacking;
			
			animHandler.ChangeAnim( velocity.x > 0 ? "AttackRight" : "AttackLeft", delegate()
	        {
				state = State.Moving;
	        });
		}*/
    }
	
	void Attacking()
	{
		//MovableCommon();
		//velocity *= velocityDampening;
    }
}
