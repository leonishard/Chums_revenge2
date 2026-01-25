using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI I { get; private set; }

    [Header("Root")]
    [SerializeField] private CanvasGroup rootGroup;
    public bool IsOpen { get; private set; }

    [Header("Left Grid")]
    [SerializeField] private Transform gridParent;
    [SerializeField] private ShopItemButton buttonPrefab;

    [Header("Right Details")]
    [SerializeField] private Image previewImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buyButtonText;
    [SerializeField] private Button closeButton;

    [Header("Behavior")]
    [SerializeField] private bool pauseTime = true;

    private VendingMachine currentMachine;
    private int selectedIndex = -1;
    private readonly List<ShopItemButton> spawned = new();
    private float oldTimeScale = 1f;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;

        if (closeButton != null) closeButton.onClick.AddListener(Close);
        if (buyButton != null) buyButton.onClick.AddListener(BuySelected);

        SetVisible(false);
    }

    private void Update()
    {
        if (!IsOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    public void Open(VendingMachine machine)
    {
        currentMachine = machine;

        BuildGrid(machine.items);

        // Auto-select first item
        if (machine.items != null && machine.items.Count > 0)
            Select(0);
        else
            Select(-1);

        SetVisible(true);
        IsOpen = true;

        if (pauseTime)
        {
            oldTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
    }

    public void Close()
    {
        if (!IsOpen) return;

        if (pauseTime)
            Time.timeScale = oldTimeScale;

        IsOpen = false;
        SetVisible(false);
        ClearGrid();
        currentMachine = null;
        selectedIndex = -1;
    }

    private void BuildGrid(List<ShopItemData> items)
    {
        ClearGrid();

        if (items == null) return;

        for (int i = 0; i < items.Count; i++)
        {
            var data = items[i];
            var btn = Instantiate(buttonPrefab, gridParent);
            btn.Bind(this, i, data);
            spawned.Add(btn);
        }
    }

    private void ClearGrid()
    {
        for (int i = 0; i < spawned.Count; i++)
        {
            if (spawned[i] != null) Destroy(spawned[i].gameObject);
        }
        spawned.Clear();
    }

    public void Select(int index)
    {
        selectedIndex = index;

        if (currentMachine == null || currentMachine.items == null ||
            index < 0 || index >= currentMachine.items.Count ||
            currentMachine.items[index] == null)
        {
            // Empty state
            if (previewImage != null) { previewImage.sprite = null; previewImage.enabled = false; }
            if (nameText != null) nameText.text = "";
            if (costText != null) costText.text = "";
            if (statsText != null) statsText.text = "";
            if (buyButton != null) buyButton.interactable = false;
            if (buyButtonText != null) buyButtonText.text = "BUY";
            return;
        }

        var item = currentMachine.items[index];

        if (previewImage != null)
        {
            previewImage.enabled = true;
            previewImage.sprite = item.icon; // expects ShopItemData.icon (Sprite)
        }

        if (nameText != null) nameText.text = item.displayName;
        if (costText != null) costText.text = $"{item.cost} coins";

        // This expects ShopItemData.statsLines OR description.
        // Adapt to your actual fields.
        if (statsText != null)
            if (statsText != null)
            {
                statsText.text =
                    $"Cost: {item.cost}\n" +
                    $"Prefab: {(item.pickupPrefab != null ? item.pickupPrefab.name : "None")}";
            }

        if (buyButton != null) buyButton.interactable = true;
        if (buyButtonText != null) buyButtonText.text = "BUY";
    }

    private void BuySelected()
    {
        if (currentMachine == null) return;
        if (selectedIndex < 0) return;

        int before = GameManager.I != null ? GameManager.I.currency : -999999; // optional, remove if you don’t have this
        currentMachine.TryBuy(selectedIndex);

        // If you want feedback without touching VendingMachine.TryBuy:
        // just pessimistically show "BOUGHT" and let your TryBuy handle fail logs.
        if (buyButtonText != null) buyButtonText.text = "BOUGHT!";
    }

    private void SetVisible(bool visible)
    {
        if (rootGroup == null) return;

        rootGroup.alpha = visible ? 1f : 0f;
        rootGroup.interactable = visible;
        rootGroup.blocksRaycasts = visible;
    }
}
