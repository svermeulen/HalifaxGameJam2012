using System;
using UnityEngine;
using System.Collections;

public class Kid : MonoBehaviour
{
    public Texture healthTexture;
    public int healthBarWidth;
    public int healthBarHeight;
    public GUIStyle textStyle;

    public AudioClip deathSound;
	public float moveSpeed = 0.1f;
    public float targetDistance = 0.1f;
	public Vector3 deathOffset;
	public Darkness darkness;
    public float offsetSetDarknessX = 1;
	
    private AnimationHandler animHandler;
	private Coyote coyote;
	
	public float movementDampening = 0;
	
	float health = 100;
	
    enum State
    {
        Idle,
		Moving,
		Dead
    }

    private State state;
	GameObject camera;
	
	// Use this for initialization
	void Start ()
	{
		camera = GameObject.FindGameObjectWithTag("MainCamera");
	    animHandler = GetComponent<AnimationHandler>();
		coyote = GameObject.FindWithTag("coyote").GetComponent<Coyote>();
		
		state = State.Moving;
    }

    float GetCameraLeftX()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera.GetComponent<Camera>());
        var rightPlanePos = -planes[0].normal * planes[0].distance;
        return rightPlanePos.x;
    }
	
	void OnGUI()
	{
        GUI.DrawTexture(new Rect(10, Screen.height - 30, healthBarWidth * (health/100.0f), healthBarHeight), healthTexture);
        GUI.Label(new Rect(10, Screen.height - 60, 100, 20), "Health", textStyle);
    }

    float GetCameraRightX()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera.GetComponent<Camera>());
        var rightPlanePos = -planes[1].normal * planes[1].distance;
        return rightPlanePos.x;
    }
	
	void ApplyDarknessDamage()
	{
        if (transform.position.x > GetCameraRightX() - offsetSetDarknessX)
		{
			TakeDamage( 1 );
		}
	}
	
	// Update is called once per frame
	void Update ()
    {
		ApplyDarknessDamage();
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
		
		rigidbody.velocity = new Vector3(rigidbody.velocity.x * movementDampening, rigidbody.velocity.y, 0);
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
		
		rigidbody.velocity = new Vector3(-moveSpeed, rigidbody.velocity.y, 0);
    }
	
	public void TakeDamage(float damage)
	{
		//Debug.Log ("take damage: " + damage);
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

                var camera = GameObject.FindGameObjectWithTag("MainCamera");
                camera.GetComponent<AudioSource>().PlayOneShot(deathSound);

				animHandler.ChangeAnim("DieLeft", delegate()
		        {
					camera.GetComponent<CameraController>().enabled = false;
					
					//darkness.ContinueGoing();
					guiObj.GetComponent<GuiHandler>().EnablePopup(GuiHandler.Popups.Death);
		        });
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "StalactPieces") { 
			StalactPieces ctl = other.gameObject.GetComponent<StalactPieces>();
			ctl.Fall();
		}
	}
	
	void OnCollisionEnter(Collision other)
	{
		//Debug.Log(other.gameObject.name);
		if (other.gameObject.name == "StalactPiece") {
			TakeDamage (100);
		}
	}
}
