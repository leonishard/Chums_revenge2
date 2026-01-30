using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip;

    private RectTransform tooltipRect;

    private void Awake()
    {
        current = this;
        tooltipRect = tooltip.GetComponent<RectTransform>();
        Hide();
    }

    public static void Show(string header, string content)
    {
        current.tooltip.headerField.text = header;
        current.tooltip.contentField.text = content;

        current.tooltip.gameObject.SetActive(true);
        current.ClampToScreen();
    }

    public static void Hide()
    {
        if (current == null) return;
        current.tooltip.gameObject.SetActive(false);
    }

    private void ClampToScreen()
    {
        Canvas canvas = tooltip.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 pos = tooltipRect.anchoredPosition;
        Vector2 size = tooltipRect.sizeDelta;

        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Horizontal clamp
        pos.x = Mathf.Clamp(
            pos.x,
            -canvasWidth / 2 + size.x,
            canvasWidth / 2
        );

        // Vertical clamp (optional but nice)
        pos.y = Mathf.Clamp(
            pos.y,
            -canvasHeight / 2,
            canvasHeight / 2 - size.y
        );

        tooltipRect.anchoredPosition = pos;
    }
}