using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI I { get; private set; }

    [Header("Root")]
    [SerializeField] private GameObject root; // panel/whole UI container

    [Header("Input Field (yellow)")]
    [SerializeField] private TMP_Text inputText; // put your yellow field text here
    [SerializeField] private int maxDigits = 4;

    [Header("Item Slots (red boxes)")]
    [SerializeField] private List<Image> slotImages = new();

    [Header("Optional")]
    [SerializeField] private TMP_Text messageText; // can be null

    public bool IsOpen => root != null && root.activeSelf;

    private VendingMachine currentMachine;
    private string currentInput = "";

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;

        if (root == null) root = gameObject;
        Close();
    }

    private void Update()
    {
        if (!IsOpen) return;

        // Confirm with keyboard R too
        if (Input.GetKeyDown(KeyCode.R))
            Confirm();

        if (Input.GetKeyDown(KeyCode.Backspace))
            Backspace();
    }

    public void Open(VendingMachine machine)
    {
        currentMachine = machine;
        currentInput = "";
        RefreshInputText();
        RefreshSlots();

        root.SetActive(true);
        ShowMessage("");
        Time.timeScale = 0f; // optional; remove if you don't want pause
    }

    public void Close()
    {
        if (root != null) root.SetActive(false);
        currentMachine = null;
        currentInput = "";
        RefreshInputText();
        ShowMessage("");
        Time.timeScale = 1f; // optional; remove if you don't want pause
    }

    // ---- Keypad button hooks ----

    public void PressDigit(int digit)
    {
        if (!IsOpen) return;
        if (currentInput.Length >= maxDigits) return;

        currentInput += Mathf.Clamp(digit, 0, 9).ToString();
        RefreshInputText();
    }

    public void Clear()
    {
        if (!IsOpen) return;
        currentInput = "";
        RefreshInputText();
    }

    public void Backspace()
    {
        if (!IsOpen) return;
        if (currentInput.Length == 0) return;

        currentInput = currentInput.Substring(0, currentInput.Length - 1);
        RefreshInputText();
    }

    public void Confirm()
    {
        if (!IsOpen) return;
        if (currentMachine == null) return;

        if (!int.TryParse(currentInput, out int code))
        {
            ShowMessage("Invalid code");
            return;
        }

        bool bought = currentMachine.TryBuyByCode(code);
        if (!bought) ShowMessage("No item / not enough coins");

        currentInput = "";
        RefreshInputText();
    }

    // ---- UI refresh ----

    private void RefreshInputText()
    {
        if (inputText) inputText.text = currentInput;
    }

    private void RefreshSlots()
    {
        if (currentMachine == null) return;

        for (int i = 0; i < slotImages.Count; i++)
        {
            Image img = slotImages[i];
            if (img == null) continue;

            if (i >= currentMachine.items.Count || currentMachine.items[i] == null)
            {
                img.enabled = false;
                continue;
            }

            var item = currentMachine.items[i];

            Sprite s = item.displaySprite;
            if (s == null && item.pickupPrefab != null)
            {
                var sr = item.pickupPrefab.GetComponentInChildren<SpriteRenderer>();
                if (sr != null) s = sr.sprite;
            }

            img.sprite = s;
            img.enabled = (s != null);
        }
    }

    private void ShowMessage(string msg)
    {
        if (messageText) messageText.text = msg;
    }
}
