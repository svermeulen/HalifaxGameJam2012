using UnityEngine;
using System.Collections;

public class SimpleFollow : MonoBehaviour
{
    Transform target;

    public float moveSpeed = 1;
    public float angleMoveSpeed = 0.1f;
    public float dampening = 0.98f;
    public bool randomizeSpeed = false;

    bool followTarget = true;

    public bool FollowTarget
    {
        get { return followTarget; }
        set { followTarget = value; }
    }
	
	void Start()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		target = player.transform;

        if (randomizeSpeed)
        {
            moveSpeed = Random.Range(0.2f, 2.0f);
        }
	}

    void Update()
    {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) 
        { 
            return; 
        }
		
        if (followTarget && target != null)
        {
            var dir = target.position - transform.position;
            dir.Normalize();

            var theta = Mathf.Atan2(dir.y, dir.x);
            var desiredRotation = Quaternion.AngleAxis((Mathf.Rad2Deg * theta) + 90, new Vector3(0, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, angleMoveSpeed);

            // Can only move in the direction we are facing
            rigidbody.velocity = (transform.rotation * new Vector3(0, -1, 0)) * moveSpeed;
        }
        else
        {
            rigidbody.velocity *= dampening;
        }
	}
}
