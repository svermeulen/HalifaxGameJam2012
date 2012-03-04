using System;
using UnityEngine;
using System.Collections;

public class Kid : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float targetDistance = 0.1f;
	public Vector3 deathOffset;

    private AnimationHandler animHandler;
	private Coyote coyote;
	
	float health = 100;
	
    enum State
    {
        Idle,
		Moving,
		Dead
    }

    private State state;
	
	// Use this for initialization
	void Start ()
	{
	    animHandler = GetComponent<AnimationHandler>();
		coyote = GameObject.FindWithTag("coyote").GetComponent<Coyote>();
		
		state = State.Moving;
	}
	
	void OnGUI()
	{
		GUI.Label( new Rect(100, 100, 100, 100), "Health: "+ health );
	}
	
	// Update is called once per frame
	void Update ()
    {
	    switch (state)
	    {
	        case State.Idle:
	            Idle();
                break;
			
	        case State.Moving:
	            Moving();
                break;
			
	        case State.Dead:
                break;

	        default:
	            throw new ArgumentOutOfRangeException();
	    }
	}
	
	void Idle()
	{
		var deltaTarget = coyote.transform.position - transform.position;
		var distance = deltaTarget.magnitude;
		
		if (distance > targetDistance && deltaTarget.x < 0)
		{
			state = State.Moving;
			animHandler.ChangeAnim("Move");
			return;
		}
	}
	
	void Moving()
	{
		var deltaTarget = coyote.transform.position - transform.position;
		var distance = deltaTarget.magnitude;
		
		var direction = deltaTarget / distance;
		direction.Normalize();
		
		if (distance <= targetDistance || deltaTarget.x >= 0)
		{
			state = State.Idle;
			animHandler.ChangeAnim("Idle");
			return;
		}

		transform.position += new Vector3(-1, 0, 0) * moveSpeed;
    }
	
	public void TakeDamage(float damage)
	{
		if (state == State.Dead)
		{
			return;
		}
		health -= damage;
		
		if (health <= 0)
		{
			var guiObj = GameObject.Find("Gui");
			
			if (guiObj)
			{				
				state = State.Dead;
				Destroy(GetComponent<Rigidbody>());
				Destroy(GetComponent<Collider>());
				transform.position -= deathOffset;
								
				animHandler.ChangeAnim("DieLeft", delegate()
		        {
					//guiObj.GetComponent<GuiHandler>().EnablePopup(GuiHandler.Popups.Death);
		        });
			}
		}
	}
}
