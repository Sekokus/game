using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        public event Action JumpFrame;
        public event Action AttackEnableFrame;
        public event Action AttackDisableFrame;
        
        [UsedImplicitly]
        public void OnJumpFrame()
        {
            JumpFrame?.Invoke();    
        }
        
        [UsedImplicitly]
        public void OnAttackEnableFrame()
        {
            AttackEnableFrame?.Invoke();    
        }
        
        [UsedImplicitly]
        public void OnAttackDisableFrame()
        {
            AttackDisableFrame?.Invoke();    
        }
    }
}