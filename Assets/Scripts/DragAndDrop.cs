using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 originalPosition, offset;
    private Quaternion originalRotation;
    private bool isDragging;
    private ToolSnap toolSnap;
    private SpriteRenderer spriteRenderer;
    private string originalSortingLayer;
    private int originalOrderInLayer;
    private const string DRAGGING_LAYER = "DraggingTool";

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        toolSnap = GetComponent<ToolSnap>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalSortingLayer = spriteRenderer.sortingLayerName;
            originalOrderInLayer = spriteRenderer.sortingOrder;
        }
    }

    private void OnMouseDown()
    {
        AudioManager.Instance.PlayToolSound();
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
        SetSortingLayer(DRAGGING_LAYER, 100);
    }

   void OnMouseDrag()
{
    if (isDragging)
    {
        Vector3 mouseWorldPos = GetMouseWorldPos() + offset;

        // Get the camera bounds
        float minX = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        float maxX = Camera.main.ViewportToWorldPoint(Vector3.one).x;
        float minY = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
        float maxY = Camera.main.ViewportToWorldPoint(Vector3.one).y;

        // Clamp the position to stay within the screen
        mouseWorldPos.x = Mathf.Clamp(mouseWorldPos.x, minX, maxX);
        mouseWorldPos.y = Mathf.Clamp(mouseWorldPos.y, minY, maxY);

        transform.position = mouseWorldPos; // Update the tool's position
    }
}


    private void OnMouseUp()
    {
        isDragging = false;
        if (toolSnap.IsCorrectlyPlaced)
        {
            toolSnap.SnapToCorrectPosition();
            AudioManager.Instance.PlaySnapSound();
            LevelManager.Instance.CheckForCompletion();
        }
        else ResetPosition();
        ResetSortingLayer();
    }

    private void ResetPosition()
    {
        AudioManager.Instance.PlayToolSound();
        transform.position = originalPosition;
        transform.rotation = originalRotation;
       
    }

    private void SetSortingLayer(string layerName, int order)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = layerName;
            spriteRenderer.sortingOrder = order;
        }
    }

    private void ResetSortingLayer() => SetSortingLayer(originalSortingLayer, originalOrderInLayer);

    private Vector3 GetMouseWorldPos() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}
