using Sekokus.Player;

namespace Sekokus
{
    public class LevelFactory
    {
        private readonly PlayerFactory _playerFactory;
        private readonly UIFactory _uiFactory;

        public LevelFactory(PlayerFactory playerFactory, UIFactory uiFactory)
        {
            _playerFactory = playerFactory;
            _uiFactory = uiFactory;
        }

        public void CreateLevel(PlayerMarker playerMarker)
        {
            _uiFactory.CreateUI();
            
            _playerFactory.CreatePlayer(playerMarker.Location);
        }
    }
}