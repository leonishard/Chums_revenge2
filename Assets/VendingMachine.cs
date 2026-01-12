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

    // debounce so one keypress can’t close+reopen
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

    public void TryBuy(int index)
    {
        if (GameManager.I == null) return;
        if (index < 0 || index >= items.Count) return;

        ShopItemData item = items[index];
        if (item == null || item.pickupPrefab == null) return;

        if (!GameManager.I.SpendCurrency(item.cost))
        {
            Debug.Log("Not enough coins!");
            return;
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}
