using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DolphinController : MonoBehaviour
{
    public LinkedSpriteManager spriteManager;
    public float moveSpeed = 0.01f;
    public float angleMoveSpeed = 0.2f;
    public float dampening = 0.95f;
	
	public float width = 1.0f;
	public float height = 1.0f;

    public float mineLaunchSpeed = 70;
    public Transform pathPointPrefab;
    public float renderPathInterval = 0.25f;

	public float finishLine = 38.5f;
	
    List<Vector3> pathPoints = new List<Vector3>();

    public Vector3 velocity;
    public float health = 100;
    bool isUpdatingFromPath = false;
    Vector3 pointOnPath = Vector3.zero;
    Vector3 lastPointOnPath = Vector3.zero;
	
	Sprite sprite;
    float currentPx = 0;
	bool isDying = false;

	void Start()
	{
		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 127, 128, 128, false);
		
		// Create and setup anims
		UVAnimation anim = new UVAnimation();
		var firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 127));
		var cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 15, 15);
		anim.name = "Idle";
		anim.loopCycles = -1;
		
		sprite.AddAnimation(anim);
		
		anim = new UVAnimation();
		firstFrame = spriteManager.PixelCoordToUVCoord(new Vector2(0, 383));
		cellSize = spriteManager.PixelSpaceToUVSpace(new Vector2(128, 128));
		anim.BuildUVAnim(firstFrame, cellSize, 8, 8, 17, 15);
		anim.name = "Die";
		anim.loopCycles = 0;
		
		sprite.AddAnimation(anim);
		
		sprite.PlayAnim("Idle");
    }

    public bool IsUpdatingFromPath
    {
        get { return isUpdatingFromPath; }
    }

    void OnEnable()
    {
		Messenger.AddListener("OnDie", OnDie);
		Messenger<bool>.AddListener("OnGameDone", OnGameDone);
    }

    void OnDisable()
    {
		Messenger.RemoveListener("OnDie", OnDie);
		Messenger<bool>.RemoveListener("OnGameDone", OnGameDone);
    }
	
	void OnGameDone(bool won)
	{
		sprite.PauseAnim();
	}
	
	void OnDie()
    {
        pathPoints.Clear();
        isUpdatingFromPath = false;

		sprite.PlayAnim("Die");
		sprite.SetAnimCompleteDelegate(delegate() 
		{ 
			ShowMenu();
		});
		
		Messenger<string>.Invoke("OnPlaySFX", "Dolphin_Scream");
	}

    public void OnDolphinPathStarted()
    {
        pathPoints.Clear();
        isUpdatingFromPath = false;
    }

    public void OnDolphinPathPointAdded(Vector3 point)
    {
        pathPoints.Add(point);
    }

    public void TakeDamage(float damage)
    {
		if (!isDying)
		{
	        health -= damage;
			health = Mathf.Clamp(health, 0.0f, 100.0f);
			
	        LoseCheck();
		}
    }

    void ShowMenu() {
		// access the PopUpMenu script
        var script = GameObject.Find("HUD").GetComponent<PopUpMenuScript>();
		
        // set the popup to the game over screen
		GameState.CurrentGameState = GameState.EGameState.GameState_Done;
		Messenger<bool>.Invoke("OnGameDone", false);
		
        script.PopupType = 0;
        script.ShowBox = true;
	}

    void LoseCheck() {
        if (!isDying && health <= 0) {
			isDying = true;
            OnDie();
        }
    }

    void WinCheck() {
        // determines the Camera limit
        float camLim = finishLine;

        // determines if the player is below the camera limit
        if (transform.position.y <= -camLim) {
            // access the PopUpMenu script
            var script = GameObject.Find("HUD").GetComponent<PopUpMenuScript>();

            // set the popup to the win menu
            script.PopupType = 1;
            script.ShowBox = true;
			
			// Win state
			GameState.CurrentGameState = GameState.EGameState.GameState_Done;
			Messenger<bool>.Invoke("OnGameDone", true);
			
			Messenger<string>.Invoke("OnPlaySong", "Victory");
        }
    }// end WinCheck method 

    static Vector3 TangentOnCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;

        return 0.5f *((3 * p3 - 9 * p2 + 9 * p1 - 3 * p0) * t2 + (-2 * p3 + 8 * p2 - 10 * p1 + 4 * p0) * t + p2 - p0);
    }

    static Vector3 PointOnCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 ret = new Vector3();

        float t2 = t * t;
        float t3 = t2 * t;

        ret = 0.5f * ((2.0f * p1) +
        (-p0 + p2) * t +
        (2.0f * p0 - 5.0f * p1 + 4 * p2 - p3) * t2 +
        (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3);

        return ret;
    }

    void RenderPath()
    {
        var lineRenderer = GetComponent<LineRenderer>();

        if (pathPoints.Count < 4)
        {
            lineRenderer.SetVertexCount(0);
        }

        float t = currentPx;

        List<Vector3> points = new List<Vector3>();
        while (t < pathPoints.Count-2)
        {
            points.Add(PointOnPath(t));
            t += renderPathInterval;
        }

        lineRenderer.SetVertexCount(points.Count);

        for (int i=0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    void OnDrawGizmos()
    {
        //Debugging.Instance.ShowText("Point count: " + pathPoints.Count);

        float t = 0;

        var lastPoint = Vector3.zero;

        foreach (var point in pathPoints)
        {
            Gizmos.DrawSphere(point, 0.05f);
        }

        /*
        while (t < pathPoints.Count-2)
        {
            var point = PointOnPath(t);
            Gizmos.DrawLine(lastPoint, point);
            lastPoint = point;

            t += 0.1f;
        }*/
    }

    Vector3 PointOnPath(float t)
    {
        Vector3 p0, p1, p2, p3;

        int curveNo = ((int)t);

        if (curveNo == 0)
        {
            p0 = pathPoints[0] + (pathPoints[0] - pathPoints[1]);
        }
        else
        {
            p0 = pathPoints[curveNo - 1];
        }

        p1 = pathPoints[curveNo];
        p2 = pathPoints[curveNo + 1];
        p3 = pathPoints[curveNo + 2];

        return PointOnCurve(p0, p1, p2, p3, t - curveNo);
    }

    Vector3 TangentOnPath(float t)
    {
        Vector3 p0, p1, p2, p3;

        int curveNo = ((int)t);

        if (curveNo == 0)
        {
            p0 = pathPoints[0] + (pathPoints[0] - pathPoints[1]);
        }
        else
        {
            p0 = pathPoints[curveNo - 1];
        }

        p1 = pathPoints[curveNo];
        p2 = pathPoints[curveNo + 1];
        p3 = pathPoints[curveNo + 2];

        return TangentOnCurve(p0, p1, p2, p3, t - curveNo);
    }

    Vector3 ClampToCameraView(Vector3 newPos)
    {
        float maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 1)).y;
        float minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1)).y;

        float extentX = collider.bounds.extents.x;
        newPos.x = Mathf.Max(extentX, Mathf.Min(7.5f - extentX, newPos.x));

        float extentY = collider.bounds.extents.y;
        newPos.y = Mathf.Max(minY + extentY, Mathf.Min(maxY - extentY, newPos.y));

        return newPos;
    }

    void UpdateFromPath()
    {
        pointOnPath = PointOnPath(currentPx);
        var desiredMoveAmount = moveSpeed * Time.deltaTime;
        var increment = 0.01f;

        while ((pointOnPath - lastPointOnPath).magnitude < desiredMoveAmount)
        {
            currentPx += increment;

            if (currentPx >= pathPoints.Count - 2)
            {
                isUpdatingFromPath = false;
                pathPoints.Clear();
                return;
            }

            pointOnPath = PointOnPath(currentPx);
        }

        velocity = (pointOnPath - lastPointOnPath) / Time.deltaTime;

        lastPointOnPath = pointOnPath;

        var newPos = ClampToCameraView(pointOnPath);
        transform.position = newPos + (transform.position - collider.bounds.center);

        var tangent = TangentOnPath(currentPx);

        var theta = Mathf.Rad2Deg * Mathf.Atan2(tangent.y, tangent.x);
        var desiredRotation = Quaternion.AngleAxis(theta + 90, new Vector3(0, 0, 1));
        
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, angleMoveSpeed);
    }

    public float PointsLeftInPath
    {
        get
        {
            return (float)pathPoints.Count - currentPx;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debugging.Instance.ShowText("Max: " + Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 1)).y
        //    + ", Min: " + Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1)).y);

		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }

        //Debugging.Instance.ShowText("Health: " + health);
        
        if (!isUpdatingFromPath && pathPoints.Count >= 2)
        {
            //pathPoints.Insert(0, transform.position);

            isUpdatingFromPath = true;
            currentPx = 0;
        }

        if (isUpdatingFromPath)
        {
            if ((int)currentPx < pathPoints.Count - 2)
            {
                UpdateFromPath();
            }
            else
            {
                isUpdatingFromPath = false;
                pathPoints.Clear();
            }
        }

        if (!isUpdatingFromPath)
        {
            var newPos = ClampToCameraView(collider.bounds.center + velocity * Time.deltaTime);
            transform.position = newPos + (transform.position - collider.bounds.center);

            velocity = velocity * dampening;
        }

        RenderPath();

        /*
        if (pathQueue.Count > 0)
        {
            var targetPos = pathQueue.Peek().position;

            var dir = targetPos - collider.bounds.center;
            dir.Normalize();

            var theta = Mathf.Atan2(dir.y, dir.x);
            var desiredRotation = Quaternion.AngleAxis((Mathf.Rad2Deg * theta) + 90, new Vector3(0, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, angleMoveSpeed);

            // Can only move in the direction we are facing
            velocity = (transform.rotation * new Vector3(0, -1, 0)) * moveSpeed;
        }
        else
        {
            velocity = velocity * dampening;
        }

        transform.position += velocity * Time.deltaTime;
        */
        WinCheck();
    }


}
