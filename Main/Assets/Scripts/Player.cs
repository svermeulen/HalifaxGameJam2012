using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public LinkedSpriteManager spriteManager;
    public GameObject spriteTransform;

    public float width = 1.0f;
    public float height = 1.0f;

    Sprite sprite;

    void Start()
    {
        sprite = spriteManager.AddSprite(spriteTransform, width, height, 0, 127, 128, 128, false);

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

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width/2, height/2, 0));
    }
}
