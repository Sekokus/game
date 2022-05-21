//public class BasicMechanicsZone : PlayerTriggerZone
//{
//    protected override void OnEnter(PlayerController player)
//    {
//        player.DashEnabled = false;
//        if (player.TryGetComponent<AttackScript>(out var attackScript))
//        {
//            attackScript.AttackEnabled = false;
//        }
//    }

//    protected override void OnExit(PlayerController player)
//    {
//        player.DashEnabled = true;
//        if (player.TryGetComponent<AttackScript>(out var attackScript))
//        {
//            attackScript.AttackEnabled = true;
//        }
//    }
//}
