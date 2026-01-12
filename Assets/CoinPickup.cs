using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private float pickupDelay = 0.05f;

    private bool canPickup;

    private void Start()
    {
        // prevents instantly collecting if it spawns inside player collider
        Invoke(nameof(EnablePickup), pickupDelay);
    }

    private void EnablePickup() => canPickup = true;

    public void SetAmount(int value) => amount = Mathf.Max(1, value);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canPickup) return;

        // tag your player "Player" OR check for a component
        if (!other.CompareTag("Player")) return;

        if (GameManager.I != null)
            GameManager.I.AddCurrency(amount);

        Destroy(gameObject);
    }
}
