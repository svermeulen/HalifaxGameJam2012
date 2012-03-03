using UnityEngine;
using System.Collections;

class Util
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
    {
        Assert(condition, "Unknown error");
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            Debug.LogError("Error: " + message + "\n" + UnityEngine.StackTraceUtility.ExtractStackTrace());
            // Pause the game until we notice this problem
            Debug.Break();
        }
    }
}