using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour 
{
    private int ekNumber = 0;

    public int EkNumber {
        get { return ekNumber; }
    }

	// Use this for initialization
	void Start () {

    }

    void OnEnable()
    {
        Messenger.AddListener("OnEnemyKilled", OnEnemyKilled);
    }

    void OnDisable()
    {
        Messenger.RemoveListener("OnEnemyKilled", OnEnemyKilled);
    }

    void OnEnemyKilled()
    {
        ekNumber++;
    }
	
    void OnGUI()
    {
        //=========================================================================================
        // Health Bar Section
        //=========================================================================================
        // Get the dolphin's health
        var hp = GameObject.Find("DolphinSprite").GetComponent<DolphinController>().health;
        Texture2D hpBG;
        hpBG = Resources.Load("HealthBar/Heathbar_bg", typeof(Texture2D)) as Texture2D;
        Texture2D hpTexture;
        hpTexture = Resources.Load("HealthBar/Heathbar_bar", typeof(Texture2D)) as Texture2D;
        

        GUIStyle style = new GUIStyle();
        style.normal.background = hpTexture;
        GUIStyle textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.black;

        int w = 124 * (int)hp / 100;

        GUI.Label(new Rect((Screen.width - 140), (Screen.height - 46), 124, 36), hpBG);
        GUI.Box(new Rect((Screen.width - 140), (Screen.height - 46), w, 36), "", style);
        //GUI.Label(new Rect((Screen.width - 70), (Screen.height - 36), 124, 36), hp.ToString(), textStyle);

        //=========================================================================================
        // Pause Button Section
        //=========================================================================================
        // Make the pause button
        if (GUI.Button(new Rect(10, (Screen.height - 30), 130, 20), "Pause")) {
            Messenger<string>.Invoke("OnPlaySFX", "Click");

            Time.timeScale = 0;
            // pauses music
            AudioListener.pause = true;

            // access the PopUpMenu script
            var script = GameObject.Find("HUD").GetComponent<PopUpMenuScript>();

            // set the popup to the pause menu
            script.PopupType = 2;
            script.ShowBox = true;

        }

        //=========================================================================================
        // Enemy Killed Score Section
        //=========================================================================================
        // Creates the label
        //GUI.Label(new Rect((10), (10), 130, 20), "Enemies killed: " + ekNumber);
    }
}
