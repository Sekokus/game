using UnityEngine;

namespace Sekokus
{
    [RequireComponent(typeof(PlayerCore))]
    public abstract class PlayerModule : MonoBehaviour
    {
        protected PlayerCore Core { get; private set; }

        private PlayerActionType[] _restrictions;

        protected void PushRestrictions(params PlayerActionType[] actions)
        {
            _restrictions = actions;
            Core.AddRestrictions(actions);
        }

        protected void PopRestrictions()
        {
            Core.RemoveRestrictions(_restrictions);
            _restrictions = null;
        }

        protected virtual void Awake()
        {
            Core = GetComponent<PlayerCore>();
        }
    }
}