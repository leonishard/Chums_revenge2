using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    private bool playerInRange;

    [Header("Items")]
    public List<ShopItemData> items = new();

    [Header("Drop")]
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float scatter = 0.25f;
    [SerializeField] private float popForceMin = 2f;
    [SerializeField] private float popForceMax = 4f;

    private float lastToggleTime = -999f;
    private const float toggleCooldown = 0.15f;

    private void Update()
    {
        if (!playerInRange) return;
        if (ShopUI.I == null) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (Time.unscaledTime - lastToggleTime < toggleCooldown) return;
            lastToggleTime = Time.unscaledTime;

            if (ShopUI.I.IsOpen) ShopUI.I.Close();
            else ShopUI.I.Open(this);
        }
    }

    public bool TryBuyByCode(int code, out string message)
    {
        message = "";

        int index = -1;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].code == code)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            message = "Invalid code";
            return false;
        }

        return TryBuy(index, out message);
    }

    public bool TryBuy(int index, out string message)
    {
        message = "";

        if (GameManager.I == null) { message = "No GameManager"; return false; }
        if (index < 0 || index >= items.Count) { message = "Invalid item"; return false; }

        ShopItemData item = items[index];
        if (item == null || item.pickupPrefab == null) { message = "Invalid item"; return false; }

        if (!GameManager.I.SpendCurrency(item.cost))
        {
            message = "Not enough!";
            return false;
        }

        Vector3 basePos = dropPoint != null ? dropPoint.position : transform.position;
        Vector3 pos = basePos + (Vector3)Random.insideUnitCircle * scatter;

        GameObject go = Instantiate(item.pickupPrefab, pos, Quaternion.identity);

        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            float force = Random.Range(popForceMin, popForceMax);
            rb.AddForce(dir * force, ForceMode2D.Impulse);
        }

        message = "Purchased";
        return true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (ShopUI.I != null && ShopUI.I.IsOpen)
            ShopUI.I.Close();
    }
}
