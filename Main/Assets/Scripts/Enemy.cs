using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float targetDistance = 0.1f;
	public float flipThreshold = 1;
	public float damageAmount = 0;
	public float hurtDistance = 0;

    private AnimationHandler animHandler;
	private Kid kid;
	
	public float maxSpeed;
		
	bool facingRight = false;

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
	}
	
	float MovableCommon()
	{
		var deltaTarget = kid.transform.position - transform.position;
		var distance = deltaTarget.magnitude;
		
		var direction = deltaTarget / distance;
		
		var rigidBody = GetComponent<Rigidbody>();
		rigidBody.AddForce(direction * moveSpeed);
		
		return distance;
	}
	
	void Moving()
	{		
		var rigidBody = GetComponent<Rigidbody>();
		
		if (facingRight)
		{
			if (rigidBody.velocity.x < -flipThreshold)
			{
				facingRight = false;
			}
		}
		else
		{
			if (rigidBody.velocity.x > flipThreshold)
			{
				facingRight = true;
			}
		}
		
		if (facingRight)
		{
			animHandler.ChangeAnim("MoveRight");
		}
		else
		{
			animHandler.ChangeAnim("MoveLeft");
		}
		
		float distance = MovableCommon();
		
		if (distance <= targetDistance)
		{
			state = State.Attacking;
			
			animHandler.ChangeAnim( facingRight ? "AttackRight" : "AttackLeft", delegate()
	        {
				FinishedAttack();
	        });
		}
    }
	
	void FinishedAttack()
	{
		float distance = (kid.transform.position - transform.position).magnitude;
		
		if (distance < hurtDistance)
		{
			kid.TakeDamage(damageAmount);
		}
		
		state = State.Moving;
	}
	
	void Attacking()
	{
		MovableCommon();
    }
}
