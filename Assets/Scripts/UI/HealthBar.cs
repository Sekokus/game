using Sekokus;

namespace UI
{
    public class HealthBar : WombImageResourceBar
    {
        protected override CharacterResource GetResource(PlayerCore player)
        {
            return player.Resources.Health;
        }
    }
}