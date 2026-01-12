using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private bool playerInRange;

    [Header("Item")]
    [SerializeField] private GameObject pickupPrefab;   // what to buy (e.g., +Damage pickup)
    [SerializeField] private int cost = 5;

    [Header("Drop")]
    [SerializeField] private Transform dropPoint;       // where it comes out
    [SerializeField] private float scatter = 0.25f;
    [SerializeField] private float popForceMin = 2f;
    [SerializeField] private float popForceMax = 4f;

    [Header("Cooldown")]
    [SerializeField] private float buyCooldown = 0.25f;
    private float lastBuyTime = -999f;

    private void Update()
    {
        if (!playerInRange) return;
        if (Time.time - lastBuyTime < buyCooldown) return;

        if (Input.GetKeyDown(interactKey))
        {
            TryBuy();
        }
    }

    private void TryBuy()
    {
        if (GameManager.I == null) return;
        if (pickupPrefab == null) return;

        if (!GameManager.I.SpendCurrency(cost))
        {
            Debug.Log("Not enough coins!");
            return;
        }

        lastBuyTime = Time.time;

        Vector3 basePos = dropPoint != null ? dropPoint.position : transform.position;
        Vector3 pos = basePos + (Vector3)Random.insideUnitCircle * scatter;

        GameObject go = Instantiate(pickupPrefab, pos, Quaternion.identity);

        // optional: pop outward a bit
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
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
