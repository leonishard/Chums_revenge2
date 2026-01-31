using UnityEngine;
using UnityEngine.EventSystems;

public class VendingItemTooltip : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item shown in this slot")]
    public ItemInfo info;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (info == null) return;

        string content = info.description;

        if (!string.IsNullOrEmpty(info.effects))
            content += "\n\nEffects: " + info.effects;

        content += "\nCode: " + info.code;

        TooltipSystem.Show(info.itemName, content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
}