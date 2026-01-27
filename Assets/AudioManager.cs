using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")] // ADD TITLE OF NEW SFX / MUSIC HERE
    public AudioClip background;
    public AudioClip coinPickUp;
    public AudioClip footstep;

    [Header("Volume")] // SET VOLUME CONTROL FOR NEW SFX / MUSIC HERE
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float coinPickupVolume = 1f;
    [Range(0f, 1f)] public float footstepVolume = 1f;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip) // ADD NEW if statement FOR EACH NEW SFX
    {
        float volume = sfxVolume;

        // Per-clip override (no refactor, explicit)
        if (clip == coinPickUp)
            volume *= coinPickupVolume;

        SFXSource.PlayOneShot(clip, volume);

        if (clip == footstep)
            volume *= footstepVolume;

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }
#endif
}
