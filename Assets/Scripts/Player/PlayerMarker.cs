using UnityEngine;

namespace Sekokus.Player
{
    public class PlayerMarker : MonoBehaviour
    {
        public Vector3 Location => transform.position;

        private void OnDrawGizmos()
        {
            GizmosHelper.DrawWithColor(Color.blue, () =>
            {
                Gizmos.DrawSphere(Location, 0.6f);
            });
        }
    }
}