using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerCore))]
    public abstract class PlayerModule : MonoBehaviour
    {
        protected PauseObserver PauseObserver { get; private set; }
        protected PlayerCore Core { get; private set; }

        private PlayerRestrictions[] _restrictions;

        protected void PushRestrictions(params PlayerRestrictions[] actions)
        {
            _restrictions = actions;
            Core.AddRestrictions(actions);
        }

        protected void PopRestrictions()
        {
            if (_restrictions == null)
            {
                return;
            }
            
            Core.RemoveRestrictions(_restrictions);
            _restrictions = null;
        }

        protected virtual void Awake()
        {
            Core = GetComponent<PlayerCore>();
            PauseObserver = Container.Get<PauseService>().GetObserver(PauseSource.Any);
        }
    }
}