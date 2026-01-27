using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        Cursor.visible = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Make sure crosshair is ALWAYS on top
        spriteRenderer.sortingLayerName = "UI"; // create this layer if it doesn't exist
        spriteRenderer.sortingOrder = 1000;
    }

    void LateUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0f;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f; // keep crosshair on its plane

        transform.position = worldPos;

        // Optional smoothing (enable if you want)
        // transform.position = Vector3.Lerp(transform.position, worldPos, 25f * Time.deltaTime);
    }
}
