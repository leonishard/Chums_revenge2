using UnityEngine;

[System.Serializable]
public class ShopItemData
{
    public string displayName;
    public int cost;
    public GameObject pickupPrefab;

    [Header("Vending Machine UI")]
    public int code;                 // the number you type (unique per item)
    public Sprite displaySprite;      // sprite shown in the red box (optional)
}
