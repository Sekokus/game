using System;
using UnityEngine;

public class ApplicationEvents : MonoBehaviour
{
    public event Action Exiting;

    private void OnApplicationQuit()
    {
        Exiting?.Invoke();
    }
}