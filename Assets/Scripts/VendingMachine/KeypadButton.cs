using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class KeypadButton : MonoBehaviour
{
    private AudioManager audioManager;
    private Button btn;

    private void Awake()
    {
        // grab AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio")
            .GetComponent<AudioManager>();

        // auto-hook button click
        btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayBeep);
    }

    private void PlayBeep()
    {
        if (audioManager == null || audioManager.machineBeep == null)
            return;

        // get SFXSource
        AudioSource sfx = audioManager.SFXSource;
        if (sfx == null)
            return;

        // apply AudioManager's machineBeep volume
        sfx.PlayOneShot(audioManager.machineBeep, audioManager.machineBeepVolume);


        // optional: random pitch
        float originalPitch = sfx.pitch;
        sfx.pitch = Random.Range(0.95f, 1.05f);


        // restore original pitch & volume
        sfx.pitch = originalPitch;
      
    }
}
