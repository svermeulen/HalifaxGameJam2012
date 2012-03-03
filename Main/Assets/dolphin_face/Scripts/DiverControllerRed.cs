using UnityEngine;
using System.Collections;

public class DiverControllerRed : DiverControllerBase 
{
    public Transform spearPrefab;
	public LinkedSpriteManager spriteManager;
    public Transform bubblesPrefab;
    public float moveSpeed = 0.01f;
    public float angleMoveSpeed = 0.2f;

	public float width = 1.0f;
	public float height = 1.0f;

    public float shootInterval = 4.0f;
    public float maxShootDistance = 10;

    public float spearThrowingSpeed = 0.5f;

	Sprite sprite;
    bool hasEnteredScreen = false;
    GameObject dolphin;
	
    public override void Start()
    {
        base.Start();

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

        dolphin = GameObject.Find("DolphinSprite");
        Util.Assert(dolphin != null);
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
	
	        Messenger<string>.Invoke("OnPlaySFX", "Diver_2");
		}
    }

    IEnumerator HandleShooting()
    {
        while (GameState.CurrentGameState == GameState.EGameState.GameState_Active)
        {
            yield return new WaitForSeconds(shootInterval);

            // Wait until close
            while ((player.transform.position - transform.position).sqrMagnitude > maxShootDistance * maxShootDistance)
            {
                yield return null;
            }

            Transform newSpear = (Transform)Instantiate(spearPrefab, Vector3.zero, Quaternion.identity);

            var dolphinDir = (dolphin.transform.position - transform.position);
            dolphinDir.Normalize();

            var spearSprite = newSpear.transform.GetChild(0);

            float theta = Mathf.Rad2Deg * Mathf.Atan2(dolphinDir.y, dolphinDir.x);

            spearSprite.position = transform.position;
            spearSprite.rotation = Quaternion.AngleAxis(theta, new Vector3(0, 0, 1));
            spearSprite.rigidbody.AddForce(dolphinDir * spearThrowingSpeed);
        }
    }

    protected override void OnActivate()
    {
    }

    public void LateUpdate()
    {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) 
        { 
            return; 
        }

        if (!isActive)
            return;

        GetComponent<SplineController>().enabled = true;
        var tangent = GetComponent<SplineInterpolator>().GetCurrentTangent();

        var theta = Mathf.Atan2(tangent.y, tangent.x);
        transform.rotation = Quaternion.AngleAxis(90 + (Mathf.Rad2Deg * theta), new Vector3(0, 0, 1));

        //Debug.DrawLine(transform.position, transform.position + tangent * 4);

        if (!hasEnteredScreen && transform.position.x < 8)
        {
            Instantiate(bubblesPrefab, transform.position + tangent * 0.2f - Vector3.forward * 0.5f, Quaternion.identity);
            StartCoroutine(HandleShooting());
            hasEnteredScreen = true;
        }

        if (GetComponent<SplineInterpolator>().IsDone() && !isDying)
        {
			isDying = true;
            OnDie();
        }

        //Debugging.Instance.ShowText("Current: " + Mathf.Abs(player.transform.position.y - transform.position.y));
	}
}
