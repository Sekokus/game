using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public event Action Exiting;
    public event Action PlayerDied;
    public event Action PlayerGoalCompleted;
    public event Action PlayerTryCollect;
    
    private void OnApplicationQuit()
    {
        Exiting?.Invoke();
    }

    public void PostPlayerDied()
    {
        PlayerDied?.Invoke();
    }

    public void PostPlayerGoalCompleted()
    {
        PlayerGoalCompleted?.Invoke();
    }

    public void PostTryCollect()
    {
        PlayerTryCollect?.Invoke();
    }
}