using UnityEngine;
using System.Collections;

public class CreateObjects : MonoBehaviour
{
    public SpriteManager spriteMan;
    public GameObject backgroundObj;

    Sprite backgroundSprite;

	// Use this for initialization
	void Start () 
    {
        backgroundSprite = spriteMan.AddSprite((GameObject)backgroundObj, // The game object to associate the sprite to
                                    768, 		// The width of the sprite
                                    4096, 		// The height of the sprite
                                    1, 		    // Left pixel
                                    1, 		    // Bottom pixel
                                    768, 		// Width in pixels
                                    4096, 		// Height in pixels
                                    false);		// Billboarded?
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
