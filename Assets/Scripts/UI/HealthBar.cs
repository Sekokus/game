using Player;

namespace UI
{
    public class HealthBar : CombImageResourceBar
    {
        protected override Resource GetResource(PlayerCore player)
        {
            return player.Resources.Health;
        }
    }
}