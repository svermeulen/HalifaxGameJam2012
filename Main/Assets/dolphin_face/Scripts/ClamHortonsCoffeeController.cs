using UnityEngine;
using System.Collections;

public class ClamHortonsCoffeeController : MonoBehaviour {

	public LinkedSpriteManager spriteManager;
    public float points = -50.0f;
	
	public float width = 1.0f;
	public float height = 1.0f;
	
	Sprite sprite;
	
	bool isOpen = false;

	// Use this for initialization
	void Start () {
		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 511, 512, 512, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 511));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(512, 512));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 16, 15);
		anim.name = "Idle";
		anim.loopCycles = -1;
		
		sprite.AddAnimation(anim);
		
		anim = new UVAnimation();
		firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 1535));
		cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(512, 512));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 19, 15);
		anim.name = "Surprise";
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
	
	void OnGameDone(bool won)
	{
		sprite.PauseAnim();
	}
	
	void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.tag == "Player" && !isOpen)
        {
			isOpen = true;
            sprite.PlayAnim("Surprise");
			Messenger<string>.Invoke("OnPlaySFX", "Surprise");
			other.gameObject.SendMessage("TakeDamage", points);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }
	}
}