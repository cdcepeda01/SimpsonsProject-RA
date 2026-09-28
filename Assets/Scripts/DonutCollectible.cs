using System.Collections;
using UnityEngine;

public class DonutCollectible : MonoBehaviour
{
    [Header("Tiempo de selección")]

    [Tooltip("Tiempo necesario mirando la dona para recogerla.")]
    [SerializeField]
    private float collectGazeTime = 0.8f;

    [Tooltip("Tiempo normal que se restaura al dejar de mirar.")]
    [SerializeField]
    private float defaultGazeTime = 2.5f;


    [Header("Audio")]

    [SerializeField]
    private AudioClip collectSound;


    [Header("Animación")]

    [Tooltip("Duración de la animación al recogerla.")]
    [SerializeField]
    private float collectAnimationDuration = 0.18f;


    private Collider interactionCollider;

    private bool collected = false;

    private Vector3 originalScale;


    private void Awake()
    {
        interactionCollider =
            GetComponent<Collider>();

        originalScale =
            transform.localScale;
    }


    // =====================================================
    // EMPIEZA A MIRAR LA DONA
    // =====================================================

    public void OnPointerEnterXR()
    {
        if (collected)
            return;

        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(
                collectGazeTime
            );
        }
    }


    // =====================================================
    // DEJA DE MIRAR LA DONA
    // =====================================================

    public void OnPointerExitXR()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(
                defaultGazeTime
            );
        }
    }


    // =====================================================
    // RECOGER DONA
    // =====================================================

    public void OnPointerClickXR()
    {
        if (collected)
            return;

        collected = true;

        StartCoroutine(
            CollectDonut()
        );
    }


    private IEnumerator CollectDonut()
    {
        // Restauramos el tiempo normal.
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(
                defaultGazeTime
            );
        }


        // Desactivamos inmediatamente el collider.
        // Así el raycast deja de detectar la dona.
        if (interactionCollider != null)
        {
            interactionCollider.enabled = false;
        }


        // Agregamos la dona al inventario.
        if (DonutInventory.Instance != null)
        {
            DonutInventory.Instance.CollectDonut();
        }
        else
        {
            Debug.LogError(
                "DonutCollectible: No existe DonutInventory en la escena."
            );
        }


        // Sonido de recolección.
        if (
            AudioManager.Instance != null &&
            collectSound != null
        )
        {
            AudioManager.Instance.PlaySFX(
                collectSound
            );
        }


        // Pequeña animación de desaparición.
        float time = 0f;

        while (time < collectAnimationDuration)
        {
            time += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    time /
                    collectAnimationDuration
                );

            transform.localScale =
                Vector3.Lerp(
                    originalScale,
                    Vector3.zero,
                    t
                );

            yield return null;
        }


        gameObject.SetActive(false);
    }
}