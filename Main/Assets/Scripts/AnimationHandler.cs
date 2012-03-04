using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AnimationInfo
{
    public Vector2 firstFrame;
    public String name;
    public int rows;
    public int columns;
    public int totalCells;
    public int fps;
    public int loopCycles;
}

public class AnimationHandler : MonoBehaviour
{
    public LinkedSpriteManager _spriteManager;
	
	public float _scaleX = 1;
	public float _scaleY = 1;
	
    public float _width = 1.0f;
    public float _height = 1.0f;
	
	public int _depthLayer = 0;
	
    public String _defaultAnimation;
    public Vector2 _cellSize;
	public float _depthScaleFactor = 1;

    public AnimationInfo[] _anims;

    Sprite _sprite;
	string _currentAnimation;

    void Start()
    {
		var transTest = new GameObject("MaterialTransform_"+ this.name);
		
		transTest.transform.position = transform.TransformPoint(transTest.transform.position);
		transTest.transform.position += new Vector3(0, 0, _depthLayer) * _depthScaleFactor;
		
		transTest.transform.localScale = new Vector3(_scaleX, _scaleY, 0.5f);
	
    	_sprite = _spriteManager.AddSprite(transTest, _width, _height, 0, (int)_cellSize.x-1, (int)_cellSize.x, (int)_cellSize.x, false);

        for (var i = 0; i < _anims.Length; i++)
        {			
            var anim = new UVAnimation();
			
			var firstFrameCoords = new Vector2(
				_anims[i].firstFrame.x * _cellSize.x, 
				_anims[i].firstFrame.y * _cellSize.y);
			
            var firstFrame = _spriteManager.PixelCoordToUVCoord(firstFrameCoords);
            var cellSize = _spriteManager.PixelSpaceToUVSpace(_cellSize);
            anim.BuildUVAnim(firstFrame, cellSize, _anims[i].columns, _anims[i].rows, _anims[i].totalCells, _anims[i].fps);
            anim.name = _anims[i].name;
            anim.loopCycles = _anims[i].loopCycles;
            _sprite.AddAnimation(anim);
        }
		
		_currentAnimation = _defaultAnimation;
        _sprite.PlayAnim(_defaultAnimation);
		
    }
	
	public string CurrentAnimation
	{
		get { return _currentAnimation; }
	}
	
    public void ChangeAnim(String anim)
    {
		if (anim == _currentAnimation)
		{
			return;
		}
		
		_currentAnimation = anim;
        _sprite.PlayAnim(anim);
    }
	
	public void FlipDirection()
	{
		//_sprite.FlipDirection();
	}

    public void ChangeAnim(String anim, Sprite.AnimCompleteDelegate callback)
    {
		_currentAnimation = anim;
        _sprite.PlayAnim(anim);
        _sprite.SetAnimCompleteDelegate(callback);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_width/2, _height/2, 0));
    }
}
