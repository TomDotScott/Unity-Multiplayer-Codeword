using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            instance ??= FindObjectOfType(typeof(T)) as T;

            instance ??= new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

            return instance;
        }
    }

    void OnApplicationQuit()
    {
        instance = null;
    }
}
