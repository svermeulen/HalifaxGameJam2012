using System;
using UnityEngine;
using System.Collections;

public class Coyote : MonoBehaviour
{
    public float moveSpeed = 0.1f;

    private AnimationHandler animHandler;
	
	const float MOVE_THRESHOLD = 0.01f;
	
	public float velocityDampening = 0.95f;
	public float maxSpeed;
	
    enum State
    {
        Idle,
        Moving,
		Turning,
		Attacking
    }

    private State state;
	bool lookingRight = true;
	Vector3 velocity;

	// Use this for initialization
	void Start ()
	{
	    state = State.Idle;
	    animHandler = GetComponent<AnimationHandler>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		CommonUpdate();
	    switch (state)
	    {
	        case State.Idle:
	            Idle();
                break;

            case State.Moving:
                Moving();
                break;

            case State.Turning:
                Turning();
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
        transform.position += velocity * Time.deltaTime;
		
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

    void Idle()
    {
        var accel = Input.GetAxis("Horizontal");
		velocity *= velocityDampening;
		
		if (lookingRight)
		{
			animHandler.ChangeAnim("IdleRight");
		}
		else
		{
			animHandler.ChangeAnim("IdleLeft");
		}
		
        if (Mathf.Abs(accel) >= MOVE_THRESHOLD)
        {
            state = State.Moving;
        }
    }

    void Moving()
    {
		MovableCommon();
		
        var accel = Input.GetAxis("Horizontal");
		velocity += new Vector3(accel, 0, 0) * moveSpeed * Time.deltaTime;
		
		if (accel < -MOVE_THRESHOLD)
		{
			if (lookingRight)
			{
				TurnLeft();
				return;
			}
			
			animHandler.ChangeAnim("MoveLeft");
		}
		else if (accel > MOVE_THRESHOLD)
		{
			if (!lookingRight)
			{
				TurnRight();
				return;
			}
			
			animHandler.ChangeAnim("MoveRight");
		}
		else
		{
            state = State.Idle;
        }
    }
	
	void TurnRight()
	{
		lookingRight = true;
		state = State.Turning;
		
		animHandler.ChangeAnim("TurnRight", delegate()
        {
			state = State.Moving;
        });
	}
	
	void TurnLeft()
	{
		lookingRight = false;
		state = State.Turning;
		
		animHandler.ChangeAnim("TurnLeft", delegate()
        {
			state = State.Moving;
        });
	}
	
	void MovableCommon()
	{
        var accel = Input.GetAxis("Horizontal");
		
		velocity += new Vector3(accel, 0, 0) * moveSpeed * Time.deltaTime;
		var speed = velocity.magnitude;
		
		if (speed > maxSpeed)
		{
			velocity = maxSpeed * velocity / speed;
		}
	}

    void Attacking()
    {
	}

    void Turning()
    {
		MovableCommon();
	}
}
