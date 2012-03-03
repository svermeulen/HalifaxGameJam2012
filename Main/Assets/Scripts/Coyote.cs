using System;
using UnityEngine;
using System.Collections;

public class Coyote : MonoBehaviour
{
    public float moveSpeed = 0.1f;

    private AnimationHandler animHandler;

    enum State
    {
        IdleRight,
        MovingRight,
        AttackingRight,
        IdleLeft,
        MovingLeft,
        AttackingLeft
    }

    private State state;

	// Use this for initialization
	void Start ()
	{
	    state = State.IdleRight;
	    animHandler = GetComponent<AnimationHandler>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		StateCommon();
	    switch (state)
	    {
	        case State.IdleLeft:
	            Idle();
                break;
			
	        case State.IdleRight:
	            Idle();
                break;

            case State.MovingRight:
                MovingRightState();
                break;

            case State.MovingLeft:
                MovingLeftState();
                break;

	        case State.AttackingLeft:
	            AttackingLeftState();
                break;

	        case State.AttackingRight:
	            AttackingRightState();
                break;

	        default:
	            throw new ArgumentOutOfRangeException();
	    }
    }
	
	bool IsLookingRight()
	{
		return state == State.AttackingRight || state == State.IdleRight || state == State.MovingRight;
	}
	
	void StateCommon()
	{
		/*
        if (Math.Abs(Input.GetAxis("Fire1")) > 0.01f)
        {
			if (IsLookingRight())
			{
	            state = State.AttackingRight;
	            animHandler.ChangeAnim("AttackRight", delegate()
	            {
	                animHandler.ChangeAnim("IdleRight");
	            });
			}
			else
			{
	            state = State.AttackingLeft;
	            animHandler.ChangeAnim("AttackLeft", delegate()
	            {
	                state = State.Idle;
	                animHandler.ChangeAnim("IdleRight");
	            });
			}
        }*/
	}

    void CheckForAttack()
    {
    }

    void Idle()
    {
        var accel = Input.GetAxis("Horizontal");
		
        if (accel > 0.01f)
        {
            state = State.MovingRight;
            animHandler.ChangeAnim("MoveRight");
        }
		else if (accel < -0.01f)
		{
            state = State.MovingLeft;
            animHandler.ChangeAnim("MoveLeft");
		}
		else
		{
        	//CheckForAttack();
		}
    }

    void MovingRightState()
    {
        var accel = Input.GetAxis("Horizontal");
        transform.position += new Vector3(accel, 0, 0) * moveSpeed;

        if (Math.Abs(accel) < 0.001f)
        {
            animHandler.ChangeAnim("IdleRight");
            state = State.IdleRight;
            return;
        }
        
        //CheckForAttack();
    }

    void MovingLeftState()
    {
        var accel = Input.GetAxis("Horizontal");
        transform.position += new Vector3(accel, 0, 0) * moveSpeed;

        if (Math.Abs(accel) < 0.001f)
        {
            animHandler.ChangeAnim("IdleLeft");
            state = State.IdleLeft;
            return;
        }
	}

    void AttackingRightState()
    {
	}

    void AttackingLeftState()
    {
    }
}
