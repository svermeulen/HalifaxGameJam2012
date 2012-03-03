using UnityEngine;
using System.Collections;

public class RockController : MonoBehaviour {

	public LinkedSpriteManager spriteManager;
	public float width = 1.0f;
	public float height = 1.0f;
	

	Sprite sprite;

	// Use this for initialization
	void Start () {
		sprite = spriteManager.AddSprite(gameObject, width, height, 0, 127, 128, 128, false);
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

	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) { return; }
	}	
}
