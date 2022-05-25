using UnityEngine;

namespace Sekokus
{
    public class UIFactory
    {
        private const string LevelUI = "LevelUI";
        private GameObject _uiPrefab;
        
        public UIFactory()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            _uiPrefab = Resources.Load<GameObject>(LevelUI);
        }

        public GameObject CreateUI()
        {
            return Object.Instantiate(_uiPrefab);
        }
    }
}