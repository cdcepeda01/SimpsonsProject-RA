using UnityEngine;

public class TeleportVisual : MonoBehaviour
{
    [Header("Pulso")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmount = 0.05f;

    [Header("Hover")]
    [SerializeField] private float hoverScale = 1.15f;
    [SerializeField] private float transitionSpeed = 8f;

    private Vector3 baseScale;
    private Vector3 targetScale;

    private bool hovered = false;

    private void Start()
    {
        baseScale = transform.localScale;
        targetScale = baseScale;
    }

    private void Update()
    {
        float pulse =
            1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        Vector3 desiredScale =
            targetScale * pulse;

        transform.localScale =
            Vector3.Lerp(
                transform.localScale,
                desiredScale,
                Time.deltaTime * transitionSpeed
            );
    }

    public void HoverEnter()
    {
        hovered = true;
        targetScale = baseScale * hoverScale;
    }

    public void HoverExit()
    {
        hovered = false;
        targetScale = baseScale;
    }
}