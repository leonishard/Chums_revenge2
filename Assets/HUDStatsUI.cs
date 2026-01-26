using TMPro;
using UnityEngine;

public class HUDStatsUI : MonoBehaviour
{
    [Header("Optional (leave empty to use GameManager.I)")]
    public GameManager gm;

    [Header("UI")]
    public TMP_Text hpText;
    public TMP_Text dmgText;
    public TMP_Text projText;
    public TMP_Text fireRateText;
    public TMP_Text ammoText;
    public TMP_Text reloadText;
    public TMP_Text currencyText;

    void Awake()
    {
        if (!gm) gm = GameManager.I ? GameManager.I : FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (!gm) return;

        if (hpText) hpText.text = $"{gm.currentHealth}/{gm.maxHealth}";
        if (dmgText) dmgText.text = $"{gm.Damage}";
        if (projText) projText.text = $"{gm.projectileCount}";
        if (fireRateText) fireRateText.text = $"{gm.timeBetweenFiring:0.00}s";
        if (ammoText) ammoText.text = $"{gm.currentAmmo}/{gm.magazineSize}";
        if (reloadText) reloadText.text = $"{gm.reloadTime:0.00}s";
        if (currencyText) currencyText.text = $"{gm.currency}";
    }
}
