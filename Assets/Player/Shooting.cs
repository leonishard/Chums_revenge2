using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;

    private PlayerStats stats;

    public GameObject bullet;
    public Transform bulletTransform;

    public bool canFire = true;
    private float timer;

    [Header("Fire Rate (lower = faster)")]
    public float timeBetweenFiring = 0.2f;

    [Header("Ammo / Reload")]
    public int magazineSize = 10;
    [SerializeField] private int currentAmmo;
    public float reloadTime = 1.2f;

    [Tooltip("Safety clamp for very slow reloads")]
    public float maxReloadTime = 5f;

    private bool isReloading = false;
    private float reloadTimer = 0f;

    [Header("Spread")]
    public float spreadDegrees = 8f;

    void Start()
    {
        mainCam = Camera.main;
        stats = GetComponentInParent<PlayerStats>();

        ApplyFromManager();

        // If manager not present or didn't set ammo yet:
        if (currentAmmo <= 0) currentAmmo = magazineSize;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, magazineSize);
    }

    public void ApplyFromManager()
    {
        if (GameManager.I == null) return;

        timeBetweenFiring = GameManager.I.timeBetweenFiring;

        // You will add these to GameManager later:
        magazineSize = GameManager.I.magazineSize;
        currentAmmo = GameManager.I.currentAmmo;
        reloadTime = GameManager.I.reloadTime;
    }

    private void SaveToManager()
    {
        if (GameManager.I == null) return;

        GameManager.I.timeBetweenFiring = timeBetweenFiring;

        // You will add these to GameManager later:
        GameManager.I.magazineSize = magazineSize;
        GameManager.I.currentAmmo = currentAmmo;
        GameManager.I.reloadTime = reloadTime;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (ShopUI.I != null && ShopUI.I.IsOpen) return;

        // Aim always (even while reloading)
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        // Reloading blocks firing
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime)
            {
                FinishReload();
            }
            return;
        }

        // Auto-reload if empty
        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        // Cooldown timer
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenFiring)
            {
                canFire = true;
                timer = 0f;
            }
        }

        // Fire
        if (Input.GetMouseButton(0) && canFire)
        {
            if (currentAmmo > 0)
            {
                canFire = false;
                Fire();
            }
            else
            {
                StartReload();
            }
        }

        // Optional manual reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }
    }

    public void AddFireRate(float amount)
    {
        // Smaller timeBetweenFiring = faster shooting
        timeBetweenFiring = Mathf.Clamp(timeBetweenFiring - amount, 0.05f, 1.5f);
        SaveToManager();
    }

    // NEW: max ammo per magazine (clip)
    public void AddMagazineSize(int amount)
    {
        magazineSize = Mathf.Max(1, magazineSize + amount);
        currentAmmo = Mathf.Clamp(currentAmmo, 0, magazineSize);
        SaveToManager();
    }

    // NEW: changes reload speed. Positive = faster (reloadTime down), negative = slower
    public void AddReloadSpeed(float amount)
    {
        reloadTime = Mathf.Clamp(reloadTime - amount, 0.1f, maxReloadTime);
        SaveToManager();
    }

    // Optional: refill ammo directly via pickups
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, magazineSize);
        SaveToManager();
    }

    private void StartReload()
    {
        if (isReloading) return;
        if (currentAmmo >= magazineSize) return; // already full

        isReloading = true;
        canFire = false;
        timer = 0f;

        reloadTimer = 0f;
        SaveToManager();
    }

    private void FinishReload()
    {
        isReloading = false;
        currentAmmo = magazineSize;

        // Let player shoot again (still respects normal cooldown)
        canFire = true;
        timer = 0f;

        SaveToManager();
    }

    private void Fire()
    {
        int count = stats ? stats.projectileCount : 1;
        int dmg = stats ? stats.Damage : 1;

        Quaternion baseRot = transform.rotation;

        for (int i = 0; i < count; i++)
        {
            float angleOffset = (count == 1) ? 0f : spreadDegrees * (i - (count - 1) / 2f);
            Quaternion rot = baseRot * Quaternion.Euler(0f, 0f, angleOffset);

            GameObject b = Instantiate(bullet, bulletTransform.position, rot);

            BulletScript bs = b.GetComponent<BulletScript>();
            if (bs != null) bs.damage = dmg;
        }

        currentAmmo--;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, magazineSize);
        SaveToManager();

        if (currentAmmo <= 0)
        {
            StartReload();
        }
    }
}
