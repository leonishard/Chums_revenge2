using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private float pickupDelay = 0.05f;

    private AudioManager audioManager;
    private bool canPickup = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")
            .GetComponent<AudioManager>();
    }

    private void Start()
    {
        
        Invoke(nameof(EnablePickup), pickupDelay);
    }

    private void EnablePickup()
    {
        canPickup = true;
    }

    public void SetAmount(int value)
    {
        amount = Mathf.Max(1, value);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canPickup) return;
        if (!other.CompareTag("Player")) return;

        if (GameManager.I != null)
            GameManager.I.AddCurrency(amount);

        
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.coinPickUp);

        Destroy(gameObject);
    }
}
