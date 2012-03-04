using System;
using UnityEngine;
using System.Collections;

public class Coyote : MonoBehaviour
{
    public float moveSpeed = 0.1f;
	public float jumpMoveSpeed = 0.1f;
	public float torqueSpring = 0.1f;
	public float rotateFriction;
	public float rotateSpring;
	public float jumpForce;
	
	public float biteRadius = 0;
	public Vector3 biteOffset;

    private AnimationHandler animHandler;
	
	const float MOVE_THRESHOLD = 0.01f;
	
	public float velocityDampening = 0.95f;
	public float maxSpeed;
	
    enum State
    {
        Idle,
        Moving,
		Turning,
		Attacking, 
		Jumping
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

	        case State.Jumping:
	            Jumping();
                break;

	        default:
	            throw new ArgumentOutOfRangeException();
	    }
    }
	
	void CommonUpdate()
	{
		Debug.DrawLine(transform.position, transform.position + desiredDirection * 1);
		Debug.DrawLine(GetBitePosition(), GetBitePosition() + Vector3.up * 1);
		
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
		
		if (Input.GetButton ("Jump")) 
		{
			if (state != State.Jumping)
			{
				StartJump();
			}
		}
		
		if (Input.GetButton("Attack")) 
		{
			if (state != State.Attacking)
			{
				StartAttack();
			}
		}
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
	
	void StartJump()
	{
		state = State.Jumping;
		
		var rigidBody = GetComponent<Rigidbody>();
		rigidBody.AddForce( new Vector3(0, jumpForce, 0) );
		
		animHandler.ChangeAnim(lookingRight ? "JumpRight" : "JumpLeft", delegate()
        {
			state = State.Moving;
        });
	}
	
	void Jumping()
	{
		MovableCommon();
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
		
		var accelSpeed = (state == State.Jumping) ? jumpMoveSpeed : moveSpeed;
		
		velocity += new Vector3(accel, 0, 0) * accelSpeed * Time.deltaTime;
		var speed = velocity.magnitude;

		if (speed > maxSpeed)
		{
			velocity = maxSpeed * velocity / speed;
		}
	}
	
	void StartAttack()
	{
		state = State.Attacking;
		
		animHandler.ChangeAnim(lookingRight ? "AttackRight" : "AttackLeft", delegate()
        {
			FinishedAttack();
        });
	}
	
	Vector3 GetBitePosition()
	{
		var bitePosition = transform.position;
		
		if (lookingRight)
		{
			bitePosition += biteOffset;
		}
		else
		{
			bitePosition -= biteOffset;	
		}
		
		return bitePosition;
	}
	
	void FinishedAttack()
	{
		foreach ( var enemyObj in GameObject.FindGameObjectsWithTag("enemy") )
		{
			var distance = (enemyObj.transform.position - GetBitePosition()).magnitude;
			
			if (distance < biteRadius)
			{
				enemyObj.GetComponent<Enemy>().Die();
			}
		}
		state = State.Idle;
	}

    void Attacking()
    {
		velocity *= velocityDampening;
	}

    void Turning()
    {
		MovableCommon();
	}
}
