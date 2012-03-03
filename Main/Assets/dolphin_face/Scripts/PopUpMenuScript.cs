using UnityEngine;
using System.Collections;

public class PopUpMenuScript : MonoBehaviour {

    //Properties
    private bool showBox; // used to show and hide the popup window
    private int popupType; // used to determine what type of screen is displayed
    private bool victoryCheck; // checks to see if the victory condition has been met.

    // Getters and Setters
    public bool ShowBox {
        get { return showBox; }
        set { showBox = value; }
    }
    public int PopupType {
        get { return popupType; }
        set { popupType = value; }
    }
    public bool VictoryCheck {
        get { return victoryCheck; }
        set { victoryCheck = value; }
    }

    // Use this for initialization
    void Start() {
        popupType = 0;
        showBox = false;
        VictoryCheck = false;
    }

    // Update is called once per frame
    void Update() {
        
        
    }

    void OnGUI() {

        if (showBox) {
            if (popupType == 0) {

                // makes it so that the defeat fanfare doesn't get trapped in an updating loop
                if (!VictoryCheck) {
                    Messenger<string>.Invoke("OnPlaySong", "Defeat");
                    VictoryCheck = true;
                }

                // Make a background box
                GUI.Box(new Rect((Screen.width / 2 - 100), (Screen.height / 2 - 60), 200, 120), "Game Over");

                // Make the restart button
                if (GUI.Button(new Rect((Screen.width / 2 - 65), (Screen.height / 2 - 30), 130, 60), "Restart Level")) {
                    Messenger<string>.Invoke("OnPlaySFX", "Click");
                    Application.LoadLevel(Application.loadedLevel);
                    AudioListener.pause = false;
                }
            }
            else if (popupType == 1) {

                // makes it so that the victory fanfare doesn't get trapped in an updating loop
                if (!VictoryCheck) {
                    Messenger<string>.Invoke("OnPlaySong", "Victory");
                    VictoryCheck = true;
                }

                int nextLevel;
                if (Application.loadedLevel == 3)
                {
                    nextLevel = 1;
                    GUI.Box(new Rect((Screen.width / 2 - 190), (Screen.height / 2 - 100), 450, 30), "Congrats! You have beaten DolphinFace!  Click to do it again!");
                }
                else
                {
                    nextLevel = Application.loadedLevel + 1;
                    GUI.Box(new Rect((Screen.width / 2 - 100), (Screen.height / 2 - 60), 200, 120), "You passed the level");
                }

                // Make the proceed button
                if (GUI.Button(new Rect((Screen.width / 2 - 65), (Screen.height / 2 - 30), 130, 60), "Proceed")) 
                {
                    Messenger<string>.Invoke("OnPlaySFX", "Click");
                    Application.LoadLevel(nextLevel);
                }
            }
            else if (popupType == 2) {
                Time.timeScale = 0;

                // darken screen
                GUI.Box(new Rect(-10, -10, (Screen.width + 20), (Screen.height +20)), "");

                // Make a background box
                GUI.Box(new Rect((Screen.width / 2 - 100), (Screen.height / 2 - 60), 200, 120), "Paused");

                // Make the proceed button
                if (GUI.Button(new Rect((Screen.width / 2 - 65), (Screen.height / 2 - 30), 130, 60), "Resume")) {
                    Messenger<string>.Invoke("OnPlaySFX", "Click");
                    Time.timeScale = 1;
                    AudioListener.pause = false;
                    ShowBox = false;
                }
            }

        }// end showBox if

    }// end OnGUI method

}
