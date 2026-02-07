using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Info")]
public class ItemInfo : ScriptableObject
{
    public string itemName;

    [TextArea(1, 2)]
    public string description;

    public int code; // price or code

    public string effects;
}
