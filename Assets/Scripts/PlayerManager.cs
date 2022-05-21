using Sekokus;

public class PlayerManager : Singleton<PlayerManager>
{
    private PlayerCore _player;

    public PlayerCore Player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<PlayerCore>();
            }

            return _player;
        }
    }
}