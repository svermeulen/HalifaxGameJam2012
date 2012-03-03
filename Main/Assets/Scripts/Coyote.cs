using System;
using UnityEngine;
using System.Collections;

public class Coyote : MonoBehaviour
{
    public float moveSpeed = 0.1f;

    private AnimationHandler animHandler;

    enum State
    {
        Idle,
        Moving,
        Attacking
    }

    private State state;

	// Use this for initialization
	void Start ()
	{
	    state = State.Idle;
	    animHandler = GetComponent<AnimationHandler>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    switch (state)
	    {
	        case State.Idle:
	            IdleState();
                break;

            case State.Moving:
                MovingState();
                break;

	        case State.Attacking:
	            AttackingState();
                break;

	        default:
	            throw new ArgumentOutOfRangeException();
	    }
    }

    void CheckForAttack()
    {
        if (Math.Abs(Input.GetAxis("Fire1")) > 0.01f)
        {
            state = State.Attacking;
            animHandler.ChangeAnim("Attack", delegate()
            {
                state = State.Idle;
                animHandler.ChangeAnim("Idle");
            });
        }
    }

    void IdleState()
    {
        var accel = Input.GetAxis("Horizontal");
        if (Math.Abs(accel) > 0.001f)
        {
            state = State.Moving;
            animHandler.ChangeAnim("Move");
            return;
        }

        CheckForAttack();
    }

    void MovingState()
    {
        var accel = Input.GetAxis("Horizontal");
        transform.position += new Vector3(accel, 0, 0) * moveSpeed;

        if (Math.Abs(accel) < 0.001f)
        {
            animHandler.ChangeAnim("Idle");
            state = State.Idle;
            return;
        }

        CheckForAttack();
    }

    void AttackingState()
    {
    }
}
