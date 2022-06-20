using UnityEngine;
using UnityEngine.InputSystem;

public class DevSaveConductor : MonoBehaviour
{
    [SerializeField] private LevelGroup[] levelGroups;
    [SerializeField] private int actionKeyPressCount = 5;
    [SerializeField] private char resetChar;
    [SerializeField] private char unlockChar;

    private int _unlockCount;
    private int _resetCount;

    private void Awake()
    {   
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        keyboard.onTextInput += OnTextInput;
    }

    private void OnDestroy()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        keyboard.onTextInput -= OnTextInput;
    }

    private void OnTextInput(char obj)
    {
        if (obj == resetChar)
        {
            if (++_resetCount == actionKeyPressCount)
            {
                ResetAllGroups();
                ResetKeys();
            }
        }
        else if (obj == unlockChar)
        {
            if (++_unlockCount == actionKeyPressCount)
            {
                UnlockAllGroups();
                ResetKeys();
            }
        }
        else
        {
            ResetKeys();
        }
    }

    private void UnlockAllGroups()
    {
        foreach (var levelGroup in levelGroups)
        {
            foreach (var levelData in levelGroup.LevelDatas)
            {
                levelData.bestLevelCoinCount = Mathf.Max(levelData.requiredCount, levelData.bestLevelCoinCount);
            }
        }
    }

    private void ResetKeys()
    {
        _resetCount = 0;
        _unlockCount = 0;
    }

    private void ResetAllGroups()
    {
        foreach (var levelGroup in levelGroups)
        {
            levelGroup.ResetPlayerDataInGroup();
        }
    }
}