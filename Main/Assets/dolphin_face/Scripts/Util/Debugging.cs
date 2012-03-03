using UnityEngine;
using System.Collections;

public class Debugging
{
    public static Debugging _instance = null;

    GameObject _dbgText;

    public static Debugging Instance
    {
        get
        {
            _instance = new Debugging();
            return _instance;
        }
    }

    private Debugging()
    {
        _dbgText = GameObject.Find("DebugText");
    }

    public void ShowText(string text)
    {
        if (_dbgText != null)
            _dbgText.guiText.text = text;
    }
}
