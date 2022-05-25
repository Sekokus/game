using Sekokus.Player;

namespace Sekokus
{
    public class LevelFactory
    {
        private readonly PlayerFactory _playerFactory;
        private readonly LevelUIFactory _levelUIFactory;

        public LevelFactory(PlayerFactory playerFactory, LevelUIFactory levelUIFactory)
        {
            _playerFactory = playerFactory;
            _levelUIFactory = levelUIFactory;
        }

        public LevelEntry CreateLevel(PlayerMarker playerMarker)
        {
            var entry = _levelUIFactory.CreateUI();
            _playerFactory.CreatePlayer(playerMarker.Location);
            return entry;
        }
    }
}