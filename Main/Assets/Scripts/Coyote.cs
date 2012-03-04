using System;
using UnityEngine;
using System.Collections;

public class Coyote : MonoBehaviour
{
    public float moveSpeed = 0.1f;
	public float torqueSpring = 0.1f;
	public float rotateFriction;
	public float rotateSpring;

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
	bool isOnFloor = false;
	public Vector3 desiredDirection = new Vector3(1, 0, 0);

	// Use this for initialization
	void Start ()
	{
	    state = State.Idle;
	    animHandler = GetComponent<AnimationHandler>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (isOnFloor)
		{
			Debug.DrawLine(transform.position, transform.position + desiredDirection * 1);
		}
		
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
		Debug.DrawLine(transform.position, transform.position + desiredDirection * 1);
		
        transform.position += velocity * Time.deltaTime;
		
		var rigidBody = GetComponent<Rigidbody>();
				
		// Add friction to rotation
		rigidBody.AddTorque(new Vector3(0, 0, - rigidBody.angularVelocity.z * rotateFriction));
		
		var desired = Mathf.Rad2Deg * Mathf.Atan2(desiredDirection.y, desiredDirection.x);
		
		var angles = rigidBody.rotation.eulerAngles;
		
		var current = angles.z;
		
		var diff = desired - current;
		
		if (Mathf.Abs(diff) > 180)
		{
			diff = desired + (360 - current);
		}
		
		rigidBody.AddTorque(new Vector3(0, 0, diff) * rotateSpring);
		
		/*
		//Vector3 dir = new Vector3(normalTest.y, -normalTest.x, normalTest.z);
		var dir = new Vector3(0, 1, 0);
		
		var theta = Mathf.Atan2(dir.y, dir.x);
		
		var rigidBody = GetComponent<Rigidbody>();
		var angles = rigidBody.rotation.eulerAngles;
		
		rigidBody.AddTorque(new Vector3(0, 0, theta-angles.z) * torqueSpring);
		*/
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
	
	void OnCollisionExit(Collision collision) 
	{
		isOnFloor = false;
		desiredDirection = new Vector3(1, 0, 0);
	}
	
	void OnCollisionStay(Collision collision) 
	{
		isOnFloor = true;
		var count = 0;
		var avgNormal = new Vector3();
		
		foreach (ContactPoint contact in collision.contacts) 
		{
			avgNormal += contact.normal;
			count += 1;
            //print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            //Debug.DrawRay(contact.point, contact.normal * 5, Color.white);
    	}
		
		avgNormal /= count;
		avgNormal.Normalize();
		
		desiredDirection = new Vector3(avgNormal.y, -avgNormal.x, avgNormal.z);
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
