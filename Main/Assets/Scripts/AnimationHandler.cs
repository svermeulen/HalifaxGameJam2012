using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AnimationInfo
{
    public Vector2 firstFrame;
    public Vector2 cellSize;
    public String name;
    public int rows;
    public int columns;
    public int totalCells;
    public int fps;
    public int loopCycles;
}

public class AnimationHandler : MonoBehaviour
{
    public LinkedSpriteManager spriteManager;
    public GameObject spriteTransform;

    public float width = 1.0f;
    public float height = 1.0f;
    public String defaultAnimation;

    public AnimationInfo[] anims;

    Sprite sprite;

    void Start()
    {
		spriteTransform.transform.position = transform.TransformPoint(spriteTransform.transform.position);
		
        sprite = spriteManager.AddSprite(spriteTransform, width, height, 0, 127, 128, 128, false);

        for (var i = 0; i < anims.Length; i++)
        {
            var anim = new UVAnimation();
            var firstFrame = spriteManager.PixelCoordToUVCoord(anims[i].firstFrame);
            var cellSize = spriteManager.PixelSpaceToUVSpace(anims[i].cellSize);
            anim.BuildUVAnim(firstFrame, cellSize, anims[i].columns, anims[i].rows, anims[i].totalCells, anims[i].fps);
            anim.name = anims[i].name;
            anim.loopCycles = anims[i].loopCycles;
            sprite.AddAnimation(anim);
        }

        sprite.PlayAnim(defaultAnimation);
		
    }

    public void ChangeAnim(String anim)
    {
        sprite.PlayAnim(anim);
    }

    public void ChangeAnim(String anim, Sprite.AnimCompleteDelegate callback)
    {
        sprite.PlayAnim(anim);
        sprite.SetAnimCompleteDelegate(callback);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width/2, height/2, 0));
    }
}
