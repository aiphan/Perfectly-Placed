using UnityEngine;

public class ToolSnap : MonoBehaviour
{
    public Transform correctSnapPoint;
    public bool IsCorrectlyPlaced { get; private set; }
    private Transform currentHoverPoint;
    private Quaternion originalRotation;
    private const float snapThreshold = 0.2f, hoverSpeed = 10f;

    private void Start() => originalRotation = transform.rotation;

    private void Update()
    {
        if (IsCorrectlyPlaced || currentHoverPoint == null) return;
        transform.position = Vector3.Lerp(transform.position, currentHoverPoint.position, Time.deltaTime * hoverSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, currentHoverPoint.rotation, Time.deltaTime * hoverSpeed);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("SnapPoint")) return;
        currentHoverPoint = col.transform;
        if (currentHoverPoint == correctSnapPoint) IsCorrectlyPlaced = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("SnapPoint")) return;
        currentHoverPoint = null;
        if (col.transform == correctSnapPoint) IsCorrectlyPlaced = false;
    }

    public void SnapToCorrectPosition()
    {
        transform.position = correctSnapPoint.position;
        transform.rotation = correctSnapPoint.rotation;
    }
}
