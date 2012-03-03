using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	
	public enum EGameState {
		GameState_Menu,
		GameState_Intro,
		GameState_Active,
		GameState_Done		
	}
	
	public static EGameState CurrentGameState { get; set; }

	// Use this for initialization
	void Start () {
		CurrentGameState = GameState.EGameState.GameState_Active;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
