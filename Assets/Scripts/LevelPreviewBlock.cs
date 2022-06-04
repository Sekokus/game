using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPreviewBlock : MonoBehaviour
{
    [SerializeField] private Image previewImage;
    [SerializeField] private LevelCompletionBar levelCompletionBar;
    [SerializeField] private TextMeshProUGUI bestLevelTimeTextMesh;
    [SerializeField] private TextMeshProUGUI levelNameTextMesh;
    [SerializeField] private LevelBlockButtonClickHandler clickHandler;

    private LevelData _levelData;
    private LevelGroupUi _groupUi;

    private void Awake()
    {
        _groupUi = Container.Get<LevelGroupUi>();

        clickHandler.AnimationEnded += () => { _groupUi.OnLevelSelected(_levelData); };
    }

    public void SetLevelData(LevelData levelData)
    {
        if (levelData == _levelData)
        {
            return;
        }

        _levelData = levelData;

        SetLevelTexts();
        SetPreviewSprite();
        SetCompletionPercentBar();
    }

    private void SetCompletionPercentBar()
    {
        levelCompletionBar.SetCompletionPercent(_levelData.CompletionPercent);
    }

    private void SetPreviewSprite()
    {
        previewImage.sprite = _levelData.previewSprite;
    }

    private void SetLevelTexts()
    {
        levelNameTextMesh.text = _levelData.levelName;
        if (!_levelData.IsFullyCompleted)
        {
            bestLevelTimeTextMesh.text = "--:--:--";
        }
        else
        {
            var time = _levelData.bestLevelTimeMs;
            bestLevelTimeTextMesh.text = $"{time / (1000_000):D2}:{time / 1000:D2}:{time % 1000:D2}";
        }
    }
}