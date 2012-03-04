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
	public float downOffset = 0;
	public float triggerDistance = 1;

    private AnimationHandler animHandler;
	private Kid kid;
	
	public float maxSpeed;
		
	bool facingRight = false;

    enum State
    {
        Moving,
		Attacking,
		Idle
    }
	
	State state;
	
	// Use this for initialization
	void Start ()
	{
	    animHandler = GetComponent<AnimationHandler>();
		kid = GameObject.FindWithTag("kid").GetComponent<Kid>();
		
		state = State.Idle;
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
			
	        case State.Idle:
	            Idle();
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
		var deltaTarget = (kid.transform.position + Vector3.down * downOffset) - transform.position;
		var distance = deltaTarget.magnitude;
		
		if (facingRight)
		{
			if (deltaTarget.x < 0)
			{
				facingRight = false;
			}
		}
		else
		{
			if (deltaTarget.x > 0)
			{
				facingRight = true;
			}
		}
		
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
	
	public void Die()
	{
		Destroy(gameObject);
	}
	
	void Idle()
	{
		float distance = (kid.transform.position - transform.position).magnitude;
		
		if (facingRight)
		{
			animHandler.ChangeAnim("IdleRight");
		}
		else
		{
			animHandler.ChangeAnim("IdleLeft");
		}
		
		if (distance < triggerDistance)
		{
			state = State.Moving;
		}
    }
	
	void Attacking()
	{
		MovableCommon();
    }
}
