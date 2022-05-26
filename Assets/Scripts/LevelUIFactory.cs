using UnityEngine;

public class LevelUIFactory
{
    private const string LevelUI = "LevelUI";
    private static GameObject _uiPrefab;
        
    public LevelUIFactory()
    {
        LoadResources();
    }

    private static void LoadResources()
    {
        _uiPrefab ??= Resources.Load<GameObject>(LevelUI);
    }

    public GameObject CreateUI()
    {
        return Object.Instantiate(_uiPrefab);
    }
}