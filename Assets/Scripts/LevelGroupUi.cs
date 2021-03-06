using System.Collections.Generic;
using UnityEngine;

public class LevelGroupUi : MonoBehaviour
{
    [SerializeField] private LevelPreviewBlock levelBlockPrefab;
    [SerializeField] private ScreenColorTint screenColorTint;

    private readonly List<LevelPreviewBlock> _createdBlocks = new List<LevelPreviewBlock>();
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = Container.Get<SceneLoader>();
    }

    public void OnLevelSelected(LevelData levelData)
    {
        _sceneLoader.ReplaceLastScene(SceneLoader.GetBuildIndex(levelData.sceneName));
    }

    public void ShowFromLevelGroup(LevelGroup levelGroup)
    {
        screenColorTint.Enable();
        
        foreach (var levelData in levelGroup.LevelDatas)
        {
            var levelBlock = Instantiate(levelBlockPrefab, transform);
            levelBlock.SetLevelData(levelData);
            _createdBlocks.Add(levelBlock);
        }
    }

    public void Hide()
    {
        // Нельзя переводить в foreach по юнитивским причинам
        for (var i = 0; i < _createdBlocks.Count; i++)
        {
            Destroy(_createdBlocks[i].gameObject);
        }

        _createdBlocks.Clear();
        screenColorTint.Disable();
    }
}