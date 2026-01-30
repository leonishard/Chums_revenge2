using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("Audio Clips")] // ADD TITLE OF NEW SFX / MUSIC HERE
    public AudioClip background;
    public AudioClip coinPickUp;
    public AudioClip footstep;
    public AudioClip machineBeep;
    public AudioClip machineOpenClose;
    public AudioClip purchased;
    public AudioClip denied;
    public AudioClip bossStep;
    public AudioClip bossSingleFire;
    public AudioClip bossShotgun;
    public AudioClip bossMusic;

    [Header("Volume")] // SET VOLUME CONTROL FOR NEW SFX / MUSIC HERE
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float coinPickupVolume = 1f;
    [Range(0f, 1f)] public float footstepVolume = 1f;
    [Range(0f, 1f)] public float machineBeepVolume = 1f;
    [Range(0f, 1f)] public float machineOpenCloseVolume = 1f;
    [Range(0f, 1f)] public float purchasedVolume = 1f;
    [Range(0f, 1f)] public float deniedVolume = 1f;
    [Range(0f, 1f)] public float bossStepVolume = 1f;
    [Range(0f, 1f)] public float bossSingleFireVolume = 1f;
    [Range(0f, 1f)] public float bossShotgunVolume = 1f;
    [Range(0f, 1f)] public float bossMusicVolume = 1f;



    private void Start()
    {
        musicSource.clip = background;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    // ✅ Public method for other scripts
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || SFXSource == null) return;

        float volume = sfxVolume;

        // Per-clip volume override
        if (clip == coinPickUp)
            volume *= coinPickupVolume;
        else if (clip == footstep)
            volume *= footstepVolume;
        else if (clip == machineBeep)
            volume *= machineBeepVolume;
        else if(clip == machineOpenClose)
            volume *= machineOpenCloseVolume;
        else if(clip == purchased)
            volume*= purchasedVolume;
        else if(clip == denied)
            volume *= deniedVolume;
        else if (clip == bossStep)
            volume *= bossStepVolume;
        else if (clip == bossSingleFire)
            volume *= bossSingleFireVolume;
        else if (clip == bossShotgun)
            volume *= bossShotgunVolume;
        else if (clip == bossMusic)
            volume *= bossMusicVolume;

        SFXSource.PlayOneShot(clip, volume);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }
#endif
}
