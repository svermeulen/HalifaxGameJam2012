using UnityEngine;
using System.Collections;

public class DiverControllerGreen : MonoBehaviour {

	public LinkedSpriteManager spriteManager;
	
	public float width = 1.0f;
	public float height = 1.0f;
	
	bool isDying = false;
	
	Sprite sprite;

	// Use this for initialization
	void Start () {
		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 127, 128, 128, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 127));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 14, 15);
		anim.name = "Idle";
		anim.loopCycles = -1;
		
		sprite.AddAnimation(anim);
		
		anim = new UVAnimation();
		firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 383));
		cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 16, 15);
		anim.name = "Die";
		anim.loopCycles = 0;
		
		sprite.AddAnimation(anim);
		
		sprite.PlayAnim("Idle");
	}
	
	void OnGameDone(bool won)
	{
		sprite.PauseAnim();
	}
	
	void OnEnable()
	{
		Messenger<bool>.AddListener("OnGameDone", OnGameDone);
	}
	
	void OnDisable()
	{
		Messenger<bool>.RemoveListener("OnGameDone", OnGameDone);
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
	
	        Messenger<string>.Invoke("OnPlaySFX", "Diver_1");
		}
    }

	
	// Update is called once per frame
	void Update()
    {
	    if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }
	}
}
