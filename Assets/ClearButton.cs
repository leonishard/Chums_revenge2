using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DeniedButtonSFX : MonoBehaviour
{
    private AudioManager audioManager;
    private Button btn;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")
            ?.GetComponent<AudioManager>();

        btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayDenied);
    }

    private void PlayDenied()
    {
        if (audioManager == null) return;
        if (audioManager.SFXSource == null) return;
        if (audioManager.denied == null) return;

        audioManager.SFXSource.PlayOneShot(
            audioManager.denied,
            audioManager.deniedVolume
        );
    }
}
