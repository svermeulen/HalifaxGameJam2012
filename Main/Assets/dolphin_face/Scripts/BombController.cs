using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombController : MonoBehaviour {
	public LinkedSpriteManager spriteManager;

	public float width = 1.0f;
	public float height = 1.0f;
	
	public float bombRadius = 1.0f;
	public float armedTime = 3.0f;
	float armedTimer = 0.0f;
	bool armed = false;
	
	Sprite sprite;

	// Use this for initialization
	void Start () {
		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 127, 128, 128, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 127));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 13, 15);
		anim.name = "Idle";
		anim.loopCycles = -1;
		
		sprite.AddAnimation(anim);
		
		anim = new UVAnimation();
		firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 127));
		cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 13, 30);
		anim.name = "Arm";
		anim.loopCycles = -1;
		
		sprite.AddAnimation(anim);
		
		anim = new UVAnimation();
		firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(256, 383));
		cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 8, 30);
		anim.name = "Explode";
		anim.loopCycles = 1;
		
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
	void Update () 
    {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }
		
		if (armed)
		{
			armedTimer -= Time.deltaTime;
			if (armedTimer <= 0.0f)
			{
				// TODO : WE EXPLODE
				GameObject[] divers = GameObject.FindGameObjectsWithTag("diver");
				for (int i = 0; i < divers.Length; ++i)
				{
					GameObject diver = divers[i];
					float distance = (diver.transform.position - transform.position).magnitude;
					
					if (distance <= bombRadius)
					{
						diver.gameObject.SendMessage("OnDie", SendMessageOptions.DontRequireReceiver);
					}
				}
				
				armed = false;
				sprite.SetSizeXY(3.0f, 3.0f);
				sprite.PlayAnim("Explode");
				sprite.SetAnimCompleteDelegate(delegate() { Destroy(transform.parent.gameObject); });
				Messenger<string>.Invoke("OnPlaySFX", "Explosion_4");
			}
		}
		
	} 
	
	void OnTriggerEnter(Collider other)
	{
		if (!armed && other.gameObject.tag == "Player")
		{
			sprite.PlayAnim("Arm");
			armedTimer = armedTime;
			armed = true;
			
			Messenger<string>.Invoke("OnPlaySFX", "Beeps");
		}
	}
}
