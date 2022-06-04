using System.Collections.Generic;
using UnityEngine;

public class LevelGroupUi : MonoBehaviour
{
    [SerializeField] private LevelPreviewBlock levelBlockPrefab;

    private readonly List<LevelPreviewBlock> _createdBlocks = new List<LevelPreviewBlock>();
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = Container.Get<SceneLoader>();
    }

    public void OnLevelSelected(LevelData levelData)
    {
        _sceneLoader.ReplaceLastScene(levelData.sceneName);
    }

    public void ShowFromLevelGroup(LevelGroup levelGroup)
    {
        foreach (var levelData in levelGroup.GetLevelDatas())
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
    }
}