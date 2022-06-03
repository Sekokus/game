using UnityEditor;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField] private Color debugColor = Color.blue;

    public Vector3 Location => transform.position;
    public Quaternion Rotation => transform.rotation;

    public virtual string Name => "Marker";
    
    private void OnDrawGizmos()
    {
        GizmosHelper.PushColor(debugColor);
        Gizmos.DrawSphere(Location, 0.5f);
        Gizmos.DrawLine(Location, Location + transform.right);
        Handles.Label(Location + Vector3.up, Name);
        GizmosHelper.PopColor();
    }
}