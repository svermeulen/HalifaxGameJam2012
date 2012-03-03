using UnityEngine;
using System.Collections;

public class BubbleFishController : MonoBehaviour {

public LinkedSpriteManager spriteManager;
	public float width = 1.0f;
	public float height = 1.0f;
	

	Sprite sprite;

	// Use this for initialization
	void Start () {
		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 511, 512, 512, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 511));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(512, 512));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 14, 15);
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
	
	// Update is called once per frame
	void Update () {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }
	}
}
