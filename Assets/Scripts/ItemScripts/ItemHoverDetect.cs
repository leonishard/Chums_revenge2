using UnityEngine;

public class ItemHoverDetector : MonoBehaviour
{
    [SerializeField] private float range = 2f;
    [SerializeField] private LayerMask itemLayer;

    private ItemPickup current;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            transform.right,
            range,
            itemLayer
        );

        if (hit.collider != null)
        {
            ItemPickup pickup = hit.collider.GetComponent<ItemPickup>();

            if (pickup != null && pickup != current)
            {
                current = pickup;

                if (pickup.info != null)
                {
                    string content = pickup.info.description;

                    if (!string.IsNullOrEmpty(pickup.info.effects))
                    {
                        content += "\n\nEffects: " + pickup.info.effects;
                    }

                    content += "\nCode: " + pickup.info.code;

                    TooltipSystem.Show(pickup.info.itemName, content);
                }
            }
        }
        else if (current != null)
        {
            current = null;
            TooltipSystem.Hide();
        }
    }
}