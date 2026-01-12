using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private string prefix = "Coins: ";

    private int lastValue = int.MinValue;

    private void Awake()
    {
        if (coinsText == null)
            coinsText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (GameManager.I == null || coinsText == null) return;

        int current = GameManager.I.currency;
        if (current == lastValue) return;

        lastValue = current;
        coinsText.text = prefix + current.ToString();
    }
}
