using System;
using System.Collections;
using DefaultNamespace;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelStartCountdown : MonoBehaviour
{
    [Space] [SerializeField] private string preCountdownText = "Click to begin";
    [SerializeField] private int startTime = 3;
    [SerializeField] private int startTextHideTime = 2;
    [SerializeField] private ScreenColorTint screenDarkener;

    [SerializeField] private TextMeshProUGUI textMesh;

    private InputBindings _bindings;

    private bool _countdownStarted;
    private bool _canStartCountdown;

    public string startText = "Begin";

    private void Awake()
    {
        Construct();
        SetPreCountdownState();
    }

    private void SetPreCountdownState()
    {
        textMesh.text = preCountdownText;
        screenDarkener.Enable();
    }

    public void SkipCountdown()
    {
        _countdownStarted = true;
        OnCountdownEnded();
    }

    private void Construct()
    {
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

        Do.After(() => textMesh.enabled = false, startTextHideTime)
            .Start(this);
        CountdownEnded?.Invoke();
    }

    public void AllowCountdownStart()
    {
        _canStartCountdown = true;
    }
}