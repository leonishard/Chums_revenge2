using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PurchaseButtonSFX : MonoBehaviour
{
    private AudioManager audioManager;
    private Button btn;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")
            ?.GetComponent<AudioManager>();

        btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayPurchased);
    }

    private void PlayPurchased()
    {
        if (audioManager == null) return;
        if (audioManager.SFXSource == null) return;
        if (audioManager.purchased == null) return;

        audioManager.SFXSource.PlayOneShot(
            audioManager.purchased,
            audioManager.purchasedVolume
        );
    }
}
