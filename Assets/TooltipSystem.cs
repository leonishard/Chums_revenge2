using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip; // assign your Tooltip GameObject here

    private void Awake()
    {
        current = this;
        if (tooltip != null) tooltip.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (tooltip != null && tooltip.gameObject.activeSelf)
        {
            // Fix tooltip to right side of screen, centered vertically
            tooltip.transform.position = new Vector3(Screen.width - 10, Screen.height / 2, 0);
        }
    }

    /// <summary>
    /// Show tooltip
    /// </summary>
    public static void Show(string header, string content)
    {
        if (current == null || current.tooltip == null) return;
        current.tooltip.SetText(header, content);
        current.tooltip.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide tooltip
    /// </summary>
    public static void Hide()
    {
        if (current == null || current.tooltip == null) return;
        current.tooltip.gameObject.SetActive(false);
    }
}
