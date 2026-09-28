using UnityEngine;
using UnityEngine.Events;

public class TeleportPoint : MonoBehaviour
{
    [Header("Eventos")]
    public UnityEvent OnTeleportEnter;
    public UnityEvent OnTeleport;
    public UnityEvent OnTeleportExit;

    [Header("Configuración del teletransporte")]

    [Tooltip("Altura de los ojos del jugador sobre el punto de teletransporte.")]
    [SerializeField]
    private float playerHeight = 1.6f;

    [Tooltip("Si está activo, el jugador adoptará la orientación del TeleportPoint.")]
    [SerializeField]
    private bool rotatePlayer = true;

    [Header("Tiempo de selección por mirada")]

    [Tooltip("Tiempo necesario mirando este TeleportPoint para activarlo.")]
    [SerializeField]
    private float teleportGazeTime = 0.8f;

    [Tooltip("Tiempo normal que se restaura para otros objetos.")]
    [SerializeField]
    private float defaultGazeTime = 2.5f;


    // =====================================================
    // CUANDO EL JUGADOR EMPIEZA A MIRAR ESTE PUNTO
    // =====================================================

    public void OnPointerEnterXR()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(teleportGazeTime);
        }

        OnTeleportEnter?.Invoke();
    }


    // =====================================================
    // CUANDO EL JUGADOR SELECCIONA ESTE PUNTO
    // =====================================================

    public void OnPointerClickXR()
    {
        ExecuteTeleportation();

        OnTeleport?.Invoke();

        // Restauramos el tiempo normal.
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(defaultGazeTime);
        }

        // Desactiva el punto utilizado y reactiva el anterior.
        if (TeleportManager.Instance != null)
        {
            TeleportManager.Instance.DisableTeleportPoint(gameObject);
        }
    }


    // =====================================================
    // CUANDO EL JUGADOR DEJA DE MIRAR ESTE PUNTO
    // =====================================================

    public void OnPointerExitXR()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(defaultGazeTime);
        }

        OnTeleportExit?.Invoke();
    }


    // =====================================================
    // EJECUTA EL TELETRANSPORTE
    // =====================================================

    private void ExecuteTeleportation()
    {
        // Verificamos que exista TeleportManager.
        if (TeleportManager.Instance == null)
        {
            Debug.LogError(
                "TeleportPoint: No se encontró TeleportManager en la escena."
            );

            return;
        }

        // Obtenemos al jugador.
        GameObject player =
            TeleportManager.Instance.Player;

        if (player == null)
        {
            Debug.LogError(
                "TeleportPoint: TeleportManager no tiene un Player asignado."
            );

            return;
        }


        // =================================================
        // POSICIÓN
        // =================================================

        // El punto puede quedarse directamente sobre el piso.
        // Al jugador le agregamos la altura de los ojos.
        Vector3 targetPosition =
            new Vector3(
                transform.position.x,
                transform.position.y + playerHeight,
                transform.position.z
            );

        player.transform.position =
            targetPosition;


        // =================================================
        // ROTACIÓN
        // =================================================

        if (rotatePlayer)
        {
            Camera playerCamera =
                player.GetComponentInChildren<Camera>();

            if (playerCamera != null)
            {
                float rotationY =
                    transform.rotation.eulerAngles.y -
                    playerCamera.transform.localEulerAngles.y;

                player.transform.rotation =
                    Quaternion.Euler(
                        0f,
                        rotationY,
                        0f
                    );
            }
        }


        // =================================================
        // SIMULADOR EN UNITY EDITOR
        // =================================================

#if UNITY_EDITOR

        CardboardSimulator simulator =
            player.GetComponent<CardboardSimulator>();

        if (simulator != null)
        {
            simulator.UpdatePlayerPositonSimulator();
        }

#endif
    }
}