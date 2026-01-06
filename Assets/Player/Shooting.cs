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
    public float timeBetweenFiring = 0.2f;

    [Header("Spread")]
    public float spreadDegrees = 8f;

    void Start()
    {
        mainCam = Camera.main;
        stats = GetComponentInParent<PlayerStats>(); // Shooting is on child/pivot, stats is on Player
    }

    void Update()
    {
        // Rotate this pivot (square) towards mouse (YOUR OLD BEHAVIOR)
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

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
            canFire = false;
            Fire();
        }
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
    }
}
