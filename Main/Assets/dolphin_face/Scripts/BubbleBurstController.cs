using UnityEngine;
using System.Collections;

public class BubbleBurstController : MonoBehaviour {

	public LinkedSpriteManager spriteManager;
    public float moveSpeed = 0.01f;
    public float angleMoveSpeed = 0.2f;
	
	public float width = 1.0f;
	public float height = 1.0f;
	
	Sprite sprite;

	// Use this for initialization
	void Start () {
		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 127, 128, 128, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 127));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 10, 15);
		anim.name = "Idle";
		anim.loopCycles = 0;
		
		sprite.AddAnimation(anim);
		sprite.PlayAnim("Idle");

        sprite.SetAnimCompleteDelegate(delegate() 
        {
            Destroy(transform.parent.gameObject); 
        });
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
