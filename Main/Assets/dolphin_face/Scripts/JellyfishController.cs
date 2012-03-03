using UnityEngine;
using System.Collections;

public class JellyfishController : MonoBehaviour {

	public LinkedSpriteManager spriteManager;
	
	public float width = 1.0f;
	public float height = 1.0f;

    public float playerDamageRate = 50;

	Sprite sprite;

	// Use this for initialization
	void Start () 
    {
		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 511, 512, 512, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 511));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(512, 512));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 24, 15);
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

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("TakeDamage", playerDamageRate * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Messenger<string>.Invoke("OnPlaySFX", "Shock2");
        }
    }
	
	// Update is called once per frame
	void LateUpdate() 
    {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }

        var splineInterp = GetComponent<SplineInterpolator>();

        if (splineInterp != null)
        {
            var tangent = GetComponent<SplineInterpolator>().GetCurrentTangent();

            var theta = Mathf.Atan2(tangent.y, tangent.x);
            transform.rotation = Quaternion.AngleAxis((Mathf.Rad2Deg * theta) - 135, new Vector3(0, 0, 1));
        }
	}
}
