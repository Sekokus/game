using System;
using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class LevelStartCountdown : MonoBehaviour
{
    [SerializeField] private bool skipCountdown = false;
    [Space] [SerializeField] private string preCountdownText = "Click to begin";
    [SerializeField] private int startTime = 3;
    [SerializeField] private int startTextHideTime = 2;
    [SerializeField] private string startText = "Begin";
    [SerializeField] private ScreenColorTint screenDarkener;

    [SerializeField] private TextMeshProUGUI textMesh;

    private CoroutineRunner _coroutineRunner;
    private PauseService _pauseService;
    private InputBindings _bindings;

    private bool _countdownStarted;
    private bool _canStartCountdown;

    private void Start()
    {
        Construct();
        SetPreCountdownState();

        if (skipCountdown)
        {
            SkipCountdown();
        }
    }

    private void SetPreCountdownState()
    {
        textMesh.text = preCountdownText;
        screenDarkener.Enable();

        _pauseService.Pause(PauseSource.LevelCountdown);
    }

    private void SkipCountdown()
    {
        _countdownStarted = true;
        OnCountdownEnded();
    }

    private void Construct()
    {
        _coroutineRunner = Container.Get<CoroutineRunner>();
        _pauseService = Container.Get<PauseService>();

        _bindings = Container.Get<PlayerBindings>().GetBindings();
        _bindings.UI.Click.performed += OnUIClick;
    }

    private void OnUIClick(InputAction.CallbackContext obj)
    {
        if (!_canStartCountdown || _countdownStarted)
        {
            return;
        }

        StartCountdown();
    }

    private void OnDisable()
    {
        _bindings.UI.Click.performed -= OnUIClick;
    }

    public event Action CountdownEnded;

    private void StartCountdown()
    {
        _countdownStarted = true;
        StartCoroutine(CountdownRoutine(startTime));
    }

    private IEnumerator CountdownRoutine(int time)
    {
        while (time > 0)
        {
            textMesh.text = time.ToString();

            yield return new WaitForSecondsRealtime(1);
            time--;
        }

        OnCountdownEnded();
    }

    private void OnCountdownEnded()
    {
        textMesh.text = startText;
        screenDarkener.Disable();
        _coroutineRunner.RunAfter(() => textMesh.enabled = false, startTextHideTime);
            
        _pauseService.Unpause(PauseSource.LevelCountdown);
            
        CountdownEnded?.Invoke();
    }

    public void AllowCountdownStart()
    {
        _canStartCountdown = true;
    }
}