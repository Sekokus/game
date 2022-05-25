using Sekokus.Player;

namespace UI
{
    public class HealthBar : WombImageResourceBar
    {
        protected override Resource GetResource(PlayerCore player)
        {
            return player.Resources.Health;
        }
    }
}