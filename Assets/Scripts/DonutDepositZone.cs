using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DonutDepositZone : MonoBehaviour
{
    [Header("Donas visuales finales")]
    [Tooltip("Donas que aparecerán sobre la bandeja al depositarlas.")]
    [SerializeField]
    private GameObject[] displayedDonuts;


    [Header("Tiempo de selección")]
    [Tooltip("Tiempo necesario mirando la bandeja para depositar las donas.")]
    [SerializeField]
    private float depositGazeTime = 0.8f;

    [Tooltip("Tiempo normal del sistema de mirada.")]
    [SerializeField]
    private float defaultGazeTime = 2.5f;


    [Header("Audio")]
    [SerializeField]
    private AudioClip depositSound;


    [Header("Eventos")]
    public UnityEvent OnDepositCompleted;


    private bool deposited = false;


    // =====================================================
    // INICIO
    // =====================================================

    private void Start()
    {
        // Ocultamos las donas finales al comenzar.
        if (displayedDonuts != null)
        {
            foreach (GameObject donut in displayedDonuts)
            {
                if (donut != null)
                {
                    donut.SetActive(false);
                }
            }
        }
    }


    // =====================================================
    // EL JUGADOR EMPIEZA A MIRAR LA ZONA
    // =====================================================

    public void OnPointerEnterXR()
    {
        if (deposited)
            return;

        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(
                depositGazeTime
            );
        }
    }


    // =====================================================
    // EL JUGADOR DEJA DE MIRAR LA ZONA
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
    // EL JUGADOR SELECCIONA LA ZONA
    // =====================================================

    public void OnPointerClickXR()
    {
        if (deposited)
            return;

        if (DonutInventory.Instance == null)
        {
            Debug.LogError(
                "DonutDepositZone: No existe DonutInventory en la escena."
            );

            return;
        }

        // Todavía no tiene todas las donas.
        if (!DonutInventory.Instance.HasAllDonuts)
        {
            Debug.Log(
                "Todavía faltan donas por recoger."
            );

            return;
        }

        deposited = true;

        StartCoroutine(
            DepositDonuts()
        );
    }


    // =====================================================
    // DEPOSITAR DONAS
    // =====================================================

    private IEnumerator DepositDonuts()
    {
        // Restauramos tiempo normal de mirada.
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(
                defaultGazeTime
            );
        }


        // Reproduce sonido.
        if (
            AudioManager.Instance != null &&
            depositSound != null
        )
        {
            AudioManager.Instance.PlaySFX(
                depositSound
            );
        }


        // Limpiamos el inventario.
        DonutInventory.Instance.ClearInventory();


        // Aparecen las donas una por una.
        if (displayedDonuts != null)
        {
            foreach (GameObject donut in displayedDonuts)
            {
                if (donut != null)
                {
                    donut.SetActive(true);

                    yield return new WaitForSeconds(
                        0.15f
                    );
                }
            }
        }


        // Avisamos que se completó el depósito.
        OnDepositCompleted?.Invoke();
    }
}