using UnityEngine;

public class GizmoDraw : MonoBehaviour
{
    private Color gizmoColor = Color.red;
    private float gizmoSize = 0.1f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoSize);
    }
}
