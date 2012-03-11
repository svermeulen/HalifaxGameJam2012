using System;
using UnityEngine;
using System.Collections;

public class Coyote : MonoBehaviour
{
	public AudioClip biteSound;
    public float moveSpeed = 0.1f;
	public float jumpMoveSpeed = 0.1f;
	public float torqueSpring = 0.1f;
	public float rotateFriction;
	public float rotateSpring;
	public float rotateFrictionGround;
	public float rotateSpringGround;
	public float jumpForce;
	public float turningMoveSpeed;
	public float maxAngleVariation = 60;
	public float gravityForce;
	public float minMoveX;
	public float maxMoveX;
	public float offsetCameraLeft;
	public GameObject dustBurst;
	
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
	
	GameObject camera;
    private State state;
	bool lookingRight = false;
	Vector3 velocity;
	bool isOnFloor = false;
	public Vector3 desiredDirection = new Vector3(1, 0, 0);
	int groundCollisionCount = 0;
	
	// Use this for initialization
	void Start ()
	{				
	    state = State.Idle;
		camera = GameObject.FindGameObjectWithTag("MainCamera");
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

	        case State.Jumping:
	            Jumping();
                break;

	        default:
	            throw new ArgumentOutOfRangeException();
	    }
    }
	
	void LimitAngles()
	{
		var rigidBody = GetComponent<Rigidbody>();
		
		var angles = rigidBody.rotation.eulerAngles;
		
		var theta = angles.z;
		
		if (theta > 180)
			theta = theta - 360;
		
		theta = Mathf.Clamp(theta, -maxAngleVariation, maxAngleVariation);
		
		rigidBody.rotation = Quaternion.AngleAxis(theta, Vector3.forward);
	}
	
	void FaceForward()
	{
		var rigidBody = GetComponent<Rigidbody>();
		
		Vector3 desDir;
		if (lookingRight)
		{
			desDir = desiredDirection;
		}
		else
		{
			desDir = -desiredDirection;
		}
		
		float rotFriction, rotSpring;
		
		if (isOnFloor)
		{
			rotFriction = rotateFrictionGround;
			rotSpring = rotateSpringGround;
		}
		else
		{
			rotFriction = rotateFriction;
			rotSpring = rotateSpring;
		}
		
		var desired = Mathf.Rad2Deg * Mathf.Atan2(desDir.y, desDir.x);
		
		rigidBody.AddTorque(new Vector3(0, 0, - rigidBody.angularVelocity.z * rotFriction));
		
		var angles = rigidBody.rotation.eulerAngles;
		
		var theta = angles.z;
		
		if (theta > 180)
			theta = theta - 360;
		
		var diff = desired - theta;
		
		//Debug.Log(diff);
		rigidBody.AddTorque(new Vector3(0, 0, diff) * rotSpring);
	}
	
	float GetCameraLeftX()
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera.GetComponent<Camera>());
		var rightPlanePos = -planes[0].normal * planes[0].distance;
		return rightPlanePos.x - offsetCameraLeft;
    }

    float GetCameraRightX()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera.GetComponent<Camera>());
        var rightPlanePos = -planes[1].normal * planes[1].distance;
        return rightPlanePos.x + offsetCameraLeft;
    }

    void ApplyPositionBoundary()
    {
        transform.position = new Vector3(Mathf.Min(maxMoveX, Mathf.Min(GetCameraRightX(), Mathf.Max(GetCameraLeftX(), Mathf.Max(minMoveX, transform.position.x)))), transform.position.y, transform.position.z);
    }

    void CommonUpdate()
	{
        Debug.DrawLine(GetBitePosition(), GetBitePosition() + Vector3.up * biteRadius, Color.blue);

        ApplyPositionBoundary();
        
		Debug.DrawLine(transform.position, transform.position + desiredDirection * 1, Color.green);
		//Debug.DrawLine(GetBitePosition(), GetBitePosition() + Vector3.up * 1);
		
		var rigidBody = GetComponent<Rigidbody>();
		rigidbody.velocity = new Vector3(velocity.x, rigidBody.velocity.y, 0);
		LimitAngles();
		
		velocity += Vector3.down * gravityForce * Time.deltaTime;
						
		if (!isOnFloor)
		{
			FaceForward();
		}
		
		if (Input.GetButton ("Jump")) 
		{
            if (isOnFloor && state != State.Jumping)
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
		velocity.x *= velocityDampening;
		
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
		
		if (accel < -MOVE_THRESHOLD)
		{
			if (isOnFloor)
			{
				if (lookingRight)
				{
					TurnLeft();
					return;
				}
				
				animHandler.ChangeAnim("MoveLeft");
			}
		}
		else if (accel > MOVE_THRESHOLD)
		{
			if (isOnFloor)
			{
				if (!lookingRight)
				{
					TurnRight();
					return;
				}
				
				animHandler.ChangeAnim("MoveRight");
			}
		}
		else
		{
            state = State.Idle;
        }
    }
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "boulder")
		{
			var rigidBody = other.transform.parent.gameObject.GetComponent<Rigidbody>();
			rigidBody.useGravity = true;
		}
	}
	
	void OnCollisionExit(Collision collision) 
	{
		if (collision.gameObject.tag == "nonground")
		{
			//return;
		}
		
		groundCollisionCount -= 1;
		
		isOnFloor = (groundCollisionCount > 0);
		
		if (lookingRight)
			desiredDirection = new Vector3(1, 0, 0);
		else
			desiredDirection = new Vector3(-1, 0, 0);
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.tag == "nonground")
		{
			//return;
		}
		
		groundCollisionCount += 1;
	}
	
	void OnCollisionStay(Collision collision) 
	{
		if (collision.gameObject.tag == "nonground")
		{
			//return;
		}
		
		isOnFloor = true;
		var count = 0;
		var avgNormal = new Vector3();
		
		foreach (ContactPoint contact in collision.contacts) 
		{
			if (contact.normal.y < 0)
				continue;
			
			avgNormal += contact.normal;
			count += 1;
            //print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            Debug.DrawRay(contact.point, contact.normal * 2, Color.red);
    	}
				
		avgNormal /= count;
		avgNormal.Normalize();
		
		desiredDirection = new Vector3(avgNormal.y, -avgNormal.x, avgNormal.z);
		
		if (!lookingRight)
			desiredDirection = -desiredDirection;
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
		rigidbody.velocity = new Vector3(velocity.x, jumpForce, 0);
		//rigidBody.AddForce( new Vector3(0, jumpForce, 0) );
		
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
		if (state == State.Jumping)
		{
			velocity += Vector3.right * Input.GetAxis("Horizontal") * jumpMoveSpeed * Time.deltaTime;
		}
		else
		{
			var accelSpeed = (state == State.Turning ? turningMoveSpeed : moveSpeed);
					
			velocity += desiredDirection * accelSpeed * Time.deltaTime;
		}
		
		var speed = velocity.magnitude;

		if (speed > maxSpeed)
		{
			velocity = maxSpeed * velocity / speed;
		}
	}
	
	void StartAttack()
	{
		camera.GetComponent<AudioSource>().PlayOneShot(biteSound, 0.1f);
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
			bitePosition += new Vector3(-biteOffset.x, biteOffset.y, 0);	
		}
		
		return bitePosition;
	}
	
	void FinishedAttack()
	{
		foreach ( var enemyObj in GameObject.FindGameObjectsWithTag("enemy") )
		{
		    var enemy = enemyObj.GetComponent<Enemy>();
            if (!enemy.HasSpawned())
            {
                continue;
            }
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
		velocity.x *= velocityDampening;
	}

    void Turning()
    {
		MovableCommon();
	}
}
