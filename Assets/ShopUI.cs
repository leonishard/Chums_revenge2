using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI I { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private TextMeshProUGUI coinsText;

    private VendingMachine currentMachine;

    public bool IsOpen => panel != null && panel.activeSelf;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;

        if (panel != null) panel.SetActive(false);
    }

    private void Update()
    {
        if (!IsOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            Close();

        if (coinsText != null && GameManager.I != null)
            coinsText.text = "Coins: " + GameManager.I.currency;
    }

    public void Open(VendingMachine machine)
    {
        if (panel == null || buttonParent == null || buttonPrefab == null) return;

        currentMachine = machine;

        // clear old buttons
        for (int i = buttonParent.childCount - 1; i >= 0; i--)
            Destroy(buttonParent.GetChild(i).gameObject);

        // create buttons
        for (int i = 0; i < currentMachine.items.Count; i++)
        {
            int index = i;
            ShopItemData item = currentMachine.items[i];

            Button b = Instantiate(buttonPrefab, buttonParent);

            var tmp = b.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = $"{item.displayName} - {item.cost}";

            b.onClick.AddListener(() => currentMachine.TryBuy(index));
        }

        panel.SetActive(true);

        // Optional: make cursor usable if your game ever locks it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Close()
    {
        if (panel == null) return;

        panel.SetActive(false);
        currentMachine = null;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
