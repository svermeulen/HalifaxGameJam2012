using UnityEngine;
using System.Collections;

public class PuffyController : MonoBehaviour {
	public LinkedSpriteManager spriteManager;
	public float width = 1.0f;
	public float height = 1.0f;
    public float angleOffset = 0;

    public float launchSpeed = 700;
    public float launchTorque = 50;
    public float dampening = 0.96f;
    public float angleDampening = 0.96f;
	
	public float blowupDistance = 5.0f;

	Sprite sprite;
    bool hasBlownUp = false;

	// Use this for initialization
	void Start () {
		sprite = spriteManager.AddSprite(gameObject, width, height, 2047, 1533, 256, 256, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 1023));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(256, 256));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 25, 15);
		anim.name = "Idle";
		anim.loopCycles = -1;
		
		sprite.AddAnimation(anim);
		
		anim = new UVAnimation();
		firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 255));
		cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(256, 256));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 20, 15);
		anim.name = "Explode";
		anim.loopCycles = 0;
		
		sprite.AddAnimation(anim);
        sprite.PlayAnim("Idle");
	}	
	
	void OnEnable()
	{
		Messenger<bool>.AddListener("OnGameDone", OnGameDone);
	}
	
	void OnDisable()
	{
		Messenger<bool>.RemoveListener("OnGameDone", OnGameDone);
	}

    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag == "Player")
        {
			GetComponent<SplineController>().enabled = false;
            GetComponent<SplineInterpolator>().enabled = false;
			
			if (!hasBlownUp)
			{
				Messenger<string>.Invoke("OnPlaySFX", "Puffer");
            	sprite.PlayAnim("Explode");
            	hasBlownUp = true;
			}
			else
			{
				Messenger<string>.Invoke("OnPlaySFX", "Puffer_Collision");
			}
			
            var dir = (transform.position - other.collider.bounds.center);
            dir.Normalize();

            rigidbody.AddForce(dir * launchSpeed);
            rigidbody.AddTorque(0, 0, launchTorque);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "diver" && hasBlownUp)
        {
            other.gameObject.SendMessage("OnDie", SendMessageOptions.DontRequireReceiver);
        }
    }
	
	void OnGameDone(bool won)
	{
		sprite.PauseAnim();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }
	}

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }

        if (GetComponent<SplineController>().enabled)
        {
            var tangent = GetComponent<SplineInterpolator>().GetCurrentTangent();

            var theta = Mathf.Atan2(tangent.y, tangent.x);
            transform.rotation = Quaternion.AngleAxis((Mathf.Rad2Deg * theta) + angleOffset, new Vector3(0, 0, 1));
        }
        else
        {
            rigidbody.velocity *= dampening;
            rigidbody.angularVelocity *= angleDampening;
        }
    }
}
