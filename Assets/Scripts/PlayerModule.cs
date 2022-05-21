using UnityEngine;

namespace Sekokus
{
    [RequireComponent(typeof(PlayerCore))]
    public abstract class PlayerModule : MonoBehaviour
    {
        protected PlayerCore Core { get; private set; }

        protected virtual void Awake()
        {
            Core = GetComponent<PlayerCore>();
        }
    }
}