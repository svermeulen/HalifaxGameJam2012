using UnityEngine;
using System.Collections;

public class DiverControllerYellow : DiverControllerBase
{
	public LinkedSpriteManager spriteManager;
	
	public float width = 1.0f;
	public float height = 1.0f;
	
	Sprite sprite;

	// Use this for initialization
	public override void Start () 
    {
        base.Start();

        GetComponent<SimpleFollow>().enabled = false;
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

    protected override void OnActivate()
    {
        GetComponent<SimpleFollow>().enabled = true;
    }
	
	// Update is called once per frame
	public override void Update()
    {
        base.Update();
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
	
	        Messenger<string>.Invoke("OnPlaySFX", "Diver_3");
		}
    }
	
	void OnGameDone(bool won)
	{
		rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		sprite.PauseAnim();
	}
}
