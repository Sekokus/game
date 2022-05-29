using UnityEngine;

namespace Player
{
    public class PlayerMarker : MonoBehaviour
    {
        public Vector3 Location => transform.position;

        private void OnDrawGizmos()
        {
            GizmosHelper.PushColor(Color.blue);

            Gizmos.DrawSphere(Location, 0.5f);
        }
    }
}