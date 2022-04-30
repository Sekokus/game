namespace UI
{
    public class HealthBar : WombImageResourceBar
    {
        protected override CharacterResource GetResource(PlayerController player)
        {
            return player.HealthResource;
        }
    }
}