using UnityEngine;
using System.Collections;

public class StarmahnController : MonoBehaviour {

	public LinkedSpriteManager spriteManager;
	public float width = 1.0f;
	public float height = 1.0f;
    public float rotateSpeed = 0;

    bool isDying = false;
	Sprite sprite;
    float rotationTheta = 0;

    public float playerDamage = 50;

	// Use this for initialization
	void Start()
    {
		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 127, 128, 128, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 127));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 1, 15);
		anim.name = "Idle";
		anim.loopCycles = -1;
		
		sprite.AddAnimation(anim);
		
		anim = new UVAnimation();
		firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 255));
		cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 13, 15);
		anim.name = "Die";
		anim.loopCycles = 0;
		
		sprite.AddAnimation(anim);
		
		sprite.PlayAnim("Idle");
	}

    void OnTriggerEnter(Collider other)
    {
        if (isDying)
            return;

        if (other.gameObject.tag == "Player")
        {
			Messenger<string>.Invoke("OnPlaySFX", "Dolphin_Damage");
            other.gameObject.SendMessage("TakeDamage", playerDamage);
        }
        else if (other.gameObject.tag == "diver")
        {
            other.gameObject.SendMessage("OnDie", SendMessageOptions.DontRequireReceiver);
        }
        else if (other.gameObject.tag == "spikemine")
        {
            if (!isDying)
                OnDie();
        }
    }

    public void OnDie()
    {
		if (!isDying)
		{
	        isDying = true;
	
	        sprite.PlayAnim("Die");
	        sprite.SetAnimCompleteDelegate(delegate()
	        {
	            Destroy(transform.parent.gameObject);
	        });
		}
    }

	void OnEnable()
	{
		Messenger<bool>.AddListener("OnGameDone", OnGameDone);
	}
	
	void OnDisable()
	{
		Messenger<bool>.RemoveListener("OnGameDone", OnGameDone);
	}
	
	void OnGameDone(bool won)
	{

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }

        transform.rotation = Quaternion.AngleAxis(rotationTheta, new Vector3(0, 0, 1));
        rotationTheta += rotateSpeed * Time.deltaTime;
	}
}
