using UnityEngine;

public class SnapPoints : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    public float gizmoSize = 0.2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoSize);
    }
}
