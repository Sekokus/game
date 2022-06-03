using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public event Action Exiting;
    public event Action PlayerDied;
    public event Action PlayerMinGoalCompleted;
    public event Action PlayerFullGoalCompleted;
    public event Action PlayerFinished;
    public event Action PlayerInteract;
    
    private void OnApplicationQuit()
    {
        Exiting?.Invoke();
    }

    public void PostPlayerDied()
    {
        PlayerDied?.Invoke();
    }

    public void PostMinGoalCompleted()
    {
        PlayerMinGoalCompleted?.Invoke();
    }

    public void PostFullGoalCompleted()
    {
        PlayerFullGoalCompleted?.Invoke();
    }

    public void PostTryCollect()
    {
        PlayerInteract?.Invoke();
    }

    public void PostFinishReached()
    {
        PlayerFinished?.Invoke();
    }
}