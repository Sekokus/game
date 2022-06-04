using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class LevelUIFactory
{
    private const string HubUi = "HubUi";
    private const string CollectAllUi = "CollectAllUi";

    private static GameObject _hubUiPrefab;
    private static GameObject _collectAllUiPrefab;

    public GameObject CreateUi(LevelType levelType)
    {
        var prefab = GetUiPrefab(levelType);
        return Object.Instantiate(prefab);
    }

    private static GameObject GetUiPrefab(LevelType levelType)
    {
        switch (levelType)
        {
            case LevelType.CollectAll:
                if (_collectAllUiPrefab == null)
                {
                    _collectAllUiPrefab = Resources.Load<GameObject>(CollectAllUi);
                }

                return _collectAllUiPrefab;
            case LevelType.Hub:
                if (_hubUiPrefab == null)
                {
                    _hubUiPrefab = Resources.Load<GameObject>(HubUi);
                }

                return _hubUiPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(levelType), levelType, null);
        }
    }
}