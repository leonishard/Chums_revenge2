using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text costText;

    private ShopUI ui;
    private int index;

    public void Bind(ShopUI ui, int index, ShopItemData data)
    {
        this.ui = ui;
        this.index = index;

        if (iconImage != null)
        {
            iconImage.sprite = data != null ? data.icon : null; // expects ShopItemData.icon
            iconImage.enabled = iconImage.sprite != null;
        }

        if (costText != null)
            costText.text = data != null ? data.cost.ToString() : "";

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (ui != null) ui.Select(index);
    }
}
