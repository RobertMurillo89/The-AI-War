using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // Lazy-loaded, thread-safe singleton instance
    private static readonly Lazy<T> _instance = new(CreateSingletonInstance, true);

    // Public property to access the singleton instance
    public static T Instance
    {
        get
        {
            return _instance.Value;
        }
    }

    // Method to create the singleton instance
    private static T CreateSingletonInstance()
    {
        lock (_instance)
        {
            // Locking to ensure thread safety during instance creation
            T instance = FindObjectOfType<T>();
            if (instance == null)
            {
                // If an instance doesn't already exist in the scene
                GameObject obj = new(typeof(T).Name);
                instance = obj.AddComponent<T>();
                // Create a new GameObject and add the component
            }
            DontDestroyOnLoad(instance.gameObject);
            // Prevent the instance from being destroyed between scenes
            return instance;
        }
    }
    protected virtual void Awake()
    {
        // Ensuring that only the first instance remains in the scene
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}

