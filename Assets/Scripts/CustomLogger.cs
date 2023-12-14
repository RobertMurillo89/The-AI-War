using UnityEngine;

public class CustomLogger
{
    static CustomLogger()
    {
    #if UNITY_EDITOR || DEVELOPMENT_BUILD
        IsDebugEnabled = true;
    #else
        IsDebugEnabled = false;
    #endif
    }

    public static bool IsDebugEnabled { get; private set; }


    public static void Log(string message)
    {
        if (IsDebugEnabled)
        {
            Debug.Log(message);
        }
    }

    public static void LogWarning(string message)
    {
        if (IsDebugEnabled)
        {
            Debug.LogWarning(message);
        }
    }

    public static void LogError(string message)
    {
        if (IsDebugEnabled)
        {
            Debug.LogError(message);
        }
    }
}
