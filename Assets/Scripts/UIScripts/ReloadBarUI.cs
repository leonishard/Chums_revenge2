using UnityEngine;
using UnityEngine.UI;

public class ReloadBarUI : MonoBehaviour
{
    public Shooting shooting;     // drag Shooting here (optional)
    public Slider slider;         // drag Slider here (optional)

    public bool hideWhenNotReloading = true;

    private CanvasGroup cg;

    void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
        cg = GetComponent<CanvasGroup>();

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0f;
        }

        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();

        SetVisible(!hideWhenNotReloading);
    }

    void Update()
    {
        if (slider == null) return;

        if (shooting == null)
        {
            shooting = FindFirstObjectByType<Shooting>();
            if (shooting == null) return;
        }

        bool show = shooting.IsReloading;

        if (hideWhenNotReloading)
            SetVisible(show);

        slider.value = show ? shooting.ReloadProgress01 : 0f;
    }

    private void SetVisible(bool visible)
    {
        if (cg == null) return;

        cg.alpha = visible ? 1f : 0f;
        cg.blocksRaycasts = visible;
        cg.interactable = visible;
    }
}
