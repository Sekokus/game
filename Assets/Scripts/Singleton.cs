using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = FindObjectOfType<T>();
            if (_instance == null)
            {
                _instance = CreateInstance();
            }

            return _instance;
        }
    }

    private static T CreateInstance()
    {
        var obj = new GameObject("Singleton")
        {
            hideFlags = HideFlags.HideAndDontSave
        };
        return obj.AddComponent<T>();
    }
}