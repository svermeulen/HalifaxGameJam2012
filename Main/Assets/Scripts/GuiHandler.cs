using UnityEngine;
using System.Collections;

public class GuiHandler : MonoBehaviour 
{
	public enum Popups
	{
		Death,
		None
	}

    public float timeScale = 1;
	Popups currentPopup = Popups.None;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	public void EnablePopup(Popups popup)
	{
		//Time.timeScale = 0;
		currentPopup = popup;
	}
	
	void Update()
	{
        Time.timeScale = timeScale;
	}
	
	// Update is called once per frame
	void OnGUI() 
	{
		switch (currentPopup)
		{
			case Popups.Death:
			{
		        if (GUI.Button(new Rect(Screen.width/2 - 100, (Screen.height/2 - 40), 130, 60), "You died. Restart?"))
				{
					Time.timeScale = 1;
		            Application.LoadLevel(0);
		        }
				break;
			}
		}
	}
}
