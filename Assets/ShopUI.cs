using System.Collections;
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
    [SerializeField] private TMP_Text inputText;
    [SerializeField] private int maxDigits = 4;

    [Header("Message")]
    [SerializeField] private float messageDuration = 1f;

    [Header("Item Slots (red boxes)")]
    [SerializeField] private List<Image> slotImages = new();

    public bool IsOpen => root != null && root.activeSelf;

    private VendingMachine currentMachine;
    private string currentInput = "";

    private Coroutine messageRoutine;
    private bool showingMessage = false;

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

        if (Input.GetKeyDown(KeyCode.R))
            Confirm();

        if (Input.GetKeyDown(KeyCode.Backspace))
            Backspace();
    }

    public void Open(VendingMachine machine)
    {
        currentMachine = machine;
        currentInput = "";
        showingMessage = false;

        RefreshSlots();
        RefreshInputText();

        root.SetActive(true);
    }

    public void Close()
    {
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageRoutine = null;

        if (root != null) root.SetActive(false);

        currentMachine = null;
        currentInput = "";
        showingMessage = false;

        if (inputText) inputText.text = "";
    }

    // ---- Keypad button hooks ----

    public void PressDigit(int digit)
    {
        if (!IsOpen) return;
        if (showingMessage) return;
        if (currentInput.Length >= maxDigits) return;

        currentInput += Mathf.Clamp(digit, 0, 9).ToString();
        RefreshInputText();
    }

    public void Clear()
    {
        if (!IsOpen) return;
        if (showingMessage) return;

        currentInput = "";
        RefreshInputText();
    }

    public void Backspace()
    {
        if (!IsOpen) return;
        if (showingMessage) return;
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
            ShowTempMessage("Invalid code");
            return;
        }

        currentMachine.TryBuyByCode(code, out string msg);
        ShowTempMessage(msg);

        currentInput = "";
    }

    // ---- UI refresh ----

    private void RefreshInputText()
    {
        if (!inputText) return;
        if (showingMessage) return;
        inputText.text = currentInput;
    }

    private void ShowTempMessage(string msg)
    {
        if (!inputText) return;

        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(TempMessageRoutine(msg));
    }

    private IEnumerator TempMessageRoutine(string msg)
    {
        showingMessage = true;
        inputText.text = msg;

        yield return new WaitForSecondsRealtime(messageDuration);

        showingMessage = false;
        inputText.text = "";
        currentInput = "";
        messageRoutine = null;
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

            // If you added displaySprite in ShopItemData:
            Sprite s = item.displaySprite;

            // Fallback: pull from prefab sprite renderer
            if (s == null && item.pickupPrefab != null)
            {
                var sr = item.pickupPrefab.GetComponentInChildren<SpriteRenderer>();
                if (sr != null) s = sr.sprite;
            }

            img.sprite = s;
            img.enabled = (s != null);
        }
    }
}
