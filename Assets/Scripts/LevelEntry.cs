using UnityEngine;

public class LevelEntry : MonoBehaviour
{
    [SerializeField] private LevelStartCountdown countdown;

    private void Awake()
    {
        countdown.CountdownEnded += OnCountdownEnded;
    }

    public void StartLevel(bool withCountdown, string startText)
    {
        Time.timeScale = 0;
        countdown.startText = startText;
        countdown.AllowCountdownStart();
        if (!withCountdown)
        {
            countdown.SkipCountdown();
        }
    }

    private void OnCountdownEnded()
    {
        Time.timeScale = 1;
    }
}