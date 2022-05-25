using UnityEngine;

namespace Sekokus
{
    public class LevelUIFactory
    {
        private const string LevelUI = "LevelUI";
        private LevelEntry _uiPrefab;
        
        public LevelUIFactory()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            _uiPrefab = Resources.Load<LevelEntry>(LevelUI);
        }

        public LevelEntry CreateUI()
        {
            return Object.Instantiate(_uiPrefab);
        }
    }
}