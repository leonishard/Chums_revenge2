using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    private Animator animator;

    private enum BossState
    {
        Idle,
        IntroMove,
        ShotgunBurst,
        Chase
    }

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Activation")]
    [SerializeField] private float activationRange = 8f;
    [SerializeField] private bool drawActivationGizmo = true;

    [Header("Health")]
    [SerializeField] private int maxHealth = 50;
    public int CurrentHealth { get; private set; }

    [Header("Movement")]
    [SerializeField] private float introMoveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 1.5f;
    [SerializeField] private float stepInterval = 0.4f;

    [Header("Shotgun")]
    [SerializeField] private int shotgunPellets = 5;
    [SerializeField] private float shotgunSpreadAngle = 35f;
    [SerializeField] private float shotgunBulletSpeed = 8f;
    [SerializeField] private float shotgunInterval = 0.6f;
    [SerializeField] private float shotgunCooldown = 15f;

    [Header("Single Shot")]
    [SerializeField] private float singleShotInterval = 2f;
    [SerializeField] private float singleBulletSpeed = 7f;

    [Header("Contact Damage")]
    public int damage = 1;

    [Header("Boss Music")]
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private float musicFadeDuration = 1.5f;

    private Transform player;
    private BossState currentState = BossState.Idle;

    private float lastSingleShotTime;
    private float lastShotgunTime;
    private float lastStepTime;

    private bool activated = false;

    private AudioManager audioManager;
    private AudioSource musicSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        CurrentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        audioManager = GameObject.FindGameObjectWithTag("Audio")
            ?.GetComponent<AudioManager>();

        if (audioManager != null)
            musicSource = audioManager.musicSource;
    }

    private void Update()
    {
        if (!activated)
        {
            animator.Play("BossIdle");

            if (player != null &&
                Vector2.Distance(transform.position, player.position) <= activationRange)
            {
                activated = true;

                // 🔥 START BOSS MUSIC
                if (musicSource != null && bossMusic != null)
                    StartCoroutine(SwapToBossMusic());

                StartCoroutine(BossRoutine());
            }

            return;
        }

        if (currentState != BossState.Chase || player == null)
            return;

        animator.Play("BossRun");
        MoveTowardPlayer(chaseSpeed);

        if (Time.time - lastSingleShotTime > singleShotInterval)
        {
            lastSingleShotTime = Time.time;
            ShootSingle();
        }

        if (Time.time - lastShotgunTime > shotgunCooldown)
        {
            lastShotgunTime = Time.time;
            StartCoroutine(ShotgunBurst(1));
        }
    }

    // ================== MUSIC ==================
    private IEnumerator SwapToBossMusic()
    {
        float startVolume = musicSource.volume;
        float t = 0f;

        // Fade OUT current BGM
        while (t < musicFadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / musicFadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = bossMusic;
        musicSource.Play();

        // Fade IN boss music
        t = 0f;
        while (t < musicFadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVolume, t / musicFadeDuration);
            yield return null;
        }

        musicSource.volume = startVolume;
    }

    // ================== BOSS ROUTINE ==================
    private IEnumerator BossRoutine()
    {
        currentState = BossState.IntroMove;
        animator.Play("BossRun");

        float introTime = 1.2f;
        float t = 0f;

        while (t < introTime)
        {
            t += Time.deltaTime;
            MoveTowardPlayer(introMoveSpeed);
            yield return null;
        }

        currentState = BossState.ShotgunBurst;
        yield return StartCoroutine(ShotgunBurst(3));

        currentState = BossState.Chase;
        lastShotgunTime = Time.time;
        lastSingleShotTime = Time.time;
    }

    // ================== MOVEMENT ==================
    private void MoveTowardPlayer(float speed)
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * speed;

        if (Time.time - lastStepTime > stepInterval)
        {
            lastStepTime = Time.time;
            PlayBossSFX(audioManager?.bossStep,
                audioManager != null ? audioManager.bossStepVolume : 1f);
        }

        if (dir.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    // ================== SHOOTING ==================
    private IEnumerator ShotgunBurst(int times)
    {
        animator.Play("BossShoot");
        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < times; i++)
        {
            FireShotgun();
            yield return new WaitForSeconds(shotgunInterval);
        }
    }

    private void FireShotgun()
    {
        if (player == null) return;

        PlayBossSFX(audioManager?.bossShotgun,
            audioManager != null ? audioManager.bossShotgunVolume : 1f);

        Vector2 baseDir = (player.position - firePoint.position).normalized;
        float startAngle = -shotgunSpreadAngle / 2f;
        float angleStep = shotgunSpreadAngle / (shotgunPellets - 1);

        for (int i = 0; i < shotgunPellets; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * baseDir;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dir * shotgunBulletSpeed;
        }
    }

    private void ShootSingle()
    {
        if (player == null) return;

        animator.Play("BossShoot");
        PlayBossSFX(audioManager?.bossSingleFire,
            audioManager != null ? audioManager.bossSingleFireVolume : 1f);

        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = dir * singleBulletSpeed;
    }

    // ================== AUDIO ==================
    private void PlayBossSFX(AudioClip clip, float volume)
    {
        if (audioManager == null || clip == null) return;
        audioManager.SFXSource.PlayOneShot(clip, volume);
    }

    // ================== HEALTH ==================
    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Boss defeated");
        Destroy(gameObject);
    }

    // ================== CONTACT DAMAGE ==================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        PlayerHealth health = collision.collider.GetComponent<PlayerHealth>();
        if (health != null)
            health.TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawActivationGizmo) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }
}
