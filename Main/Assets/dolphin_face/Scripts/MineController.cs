using UnityEngine;
using System.Collections;

public class MineController : MonoBehaviour 
{
	public LinkedSpriteManager spriteManager;

	public float width = 1.0f;
    public float height = 1.0f;
    public float launchSpeed = 750;
    public float dampening = 1;

	Sprite sprite;

	// Use this for initialization
	void Start()
    {
        width = collider.bounds.size.x * 2 + 0.15f;
        height = collider.bounds.size.y * 2 + 0.15f;

		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 127, 128, 128, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 127));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 11, 15);
		anim.name = "Idle";
		anim.loopCycles = -1;
		
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
	
	void OnGameDone(bool won)
	{
		sprite.PauseAnim();
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var dir = (transform.position - other.collider.bounds.center);
            dir.Normalize();

            rigidbody.AddForce(dir * launchSpeed);
        }
        else if (other.gameObject.tag == "diver")
        {
            other.gameObject.SendMessage("OnDie", SendMessageOptions.DontRequireReceiver);
        }
		else if (other.gameObject.tag == "mine")
		{
			Messenger<string>.Invoke("OnPlaySFX", "Thunk");
		}
    }
	
	// Update is called once per frame
	void Update () {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }

        rigidbody.velocity *= dampening;
	}
}
