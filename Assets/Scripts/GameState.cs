using System;
using UnityEngine;

public delegate void GameStateChanged(GameStateType newState, GameStateType oldState);

public class GameState : MonoBehaviour
{
    public GameStateType CurrentState { get; private set; } = GameStateType.Default;

    public void SetState(GameStateType state)
    {
        var previousState = CurrentState;
        CurrentState = state;
        StateChanged?.Invoke(state, previousState);
    }

    public event GameStateChanged StateChanged;

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