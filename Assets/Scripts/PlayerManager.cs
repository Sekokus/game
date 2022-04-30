public class PlayerManager : Singleton<PlayerManager>
{
    private PlayerController _player;

    public PlayerController Player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<PlayerController>();
            }

            return _player;
        }
    }
}