using UnityEngine;
using System.Collections;

public class GameStartMenuScript : MonoBehaviour {

    // Use this for initialization
    void Start() {
        StartCoroutine(Fade(1.0F, 1.0f));
    }

    // Update is called once per frame
    void Update() {

    }

    void OnGUI() {


        // Make the start button
        if (GUI.Button(new Rect((Screen.width -180) , (Screen.height/2 + 180), 130, 60), "Start")) {
            // Play Button Click noise
            Messenger<string>.Invoke("OnPlaySFX", "Click");
            StartCoroutine(Fade(1.0F, 0.2f));
            // set start level
            Application.LoadLevel(1);
            
        }

    }// end OnGUI method

    IEnumerator Fade(float waitTime, float volume) {
        yield return new WaitForSeconds(waitTime);
        Messenger<float>.Invoke("OnFade", volume);
    }

}
