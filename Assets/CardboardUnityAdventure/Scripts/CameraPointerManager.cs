using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public static CameraPointerManager Instance;

    [Header("Pointer")]
    [SerializeField] private GameObject pointer;

    [Tooltip("Distancia del puntero cuando no se apunta a un objeto interactuable")]
    [SerializeField] private float maxDistancePointer = 4.5f;

    [Range(0f, 1f)]
    [Tooltip("Qué tan cerca del objeto se coloca el puntero al apuntarlo")]
    [SerializeField] private float disPointerObject = 0.95f;

    [Tooltip("Tamaño del punto cuando no se apunta a un objeto interactuable")]
    [SerializeField] private float idlePointerScale = 0.15f;

    [Tooltip("Tamaño base del puntero cuando está sobre un objeto interactuable")]
    [SerializeField] private float interactablePointerScale = 0.025f;

    [Header("Raycast")]
    [SerializeField] private float maxRaycastDistance = 10f;

    private GameObject _gazedAtObject = null;

    private const string InteractableTag = "Interactable";

    [HideInInspector]
    public Vector3 hitPoint;


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.OnGazeSelection += GazeSelection;
        }
        else
        {
            Debug.LogError(
                "CameraPointerManager: No se encontró un GazeManager en la escena."
            );
        }

        // Al comenzar, dejamos el punto visible
        // directamente frente a la cámara.
        PointerOutGaze();
    }


    // =========================================================
    // ON DESTROY
    // =========================================================

    private void OnDestroy()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.OnGazeSelection -= GazeSelection;
        }

        if (Instance == this)
        {
            Instance = null;
        }
    }


    // =========================================================
    // SELECCIÓN MEDIANTE MIRADA
    // =========================================================

    private void GazeSelection()
    {
        if (_gazedAtObject == null)
            return;

        if (!_gazedAtObject.CompareTag(InteractableTag))
            return;

        _gazedAtObject.SendMessage(
            "OnPointerClickXR",
            null,
            SendMessageOptions.DontRequireReceiver
        );
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        RaycastHit hit;

        bool hasHit = Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            maxRaycastDistance
        );

        if (hasHit)
        {
            hitPoint = hit.point;

            GameObject hitObject = hit.transform.gameObject;

            // -------------------------------------------------
            // Cambiamos de objeto observado
            // -------------------------------------------------

            if (_gazedAtObject != hitObject)
            {
                // Avisamos al objeto anterior que dejamos de mirarlo.
                if (_gazedAtObject != null)
                {
                    _gazedAtObject.SendMessage(
                        "OnPointerExitXR",
                        null,
                        SendMessageOptions.DontRequireReceiver
                    );
                }

                // Guardamos el nuevo objeto.
                _gazedAtObject = hitObject;

                // Avisamos al nuevo objeto que lo estamos mirando.
                _gazedAtObject.SendMessage(
                    "OnPointerEnterXR",
                    null,
                    SendMessageOptions.DontRequireReceiver
                );

                // Si es interactuable, comenzamos el temporizador.
                if (_gazedAtObject.CompareTag(InteractableTag))
                {
                    if (GazeManager.Instance != null)
                    {
                        GazeManager.Instance.StartGazeSelection();
                    }
                }
                else
                {
                    if (GazeManager.Instance != null)
                    {
                        GazeManager.Instance.CancelGazeSelection();
                    }
                }
            }


            // -------------------------------------------------
            // Posición visual del puntero
            // -------------------------------------------------

            if (hitObject.CompareTag(InteractableTag))
            {
                // El punto se coloca sobre el botón u objeto.
                PointerOnGaze(hit.point);
            }
            else
            {
                // Estamos mirando algo no interactuable:
                // mantenemos un punto fijo delante de la cámara.
                PointerOutGaze();
            }
        }
        else
        {
            // -------------------------------------------------
            // No estamos mirando ningún objeto
            // -------------------------------------------------

            if (_gazedAtObject != null)
            {
                _gazedAtObject.SendMessage(
                    "OnPointerExitXR",
                    null,
                    SendMessageOptions.DontRequireReceiver
                );
            }

            _gazedAtObject = null;

            // El cursor continúa visible delante de la cámara.
            PointerOutGaze();
        }


        // =====================================================
        // TRIGGER FÍSICO DE GOOGLE CARDBOARD
        // =====================================================

        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            if (
                _gazedAtObject != null &&
                _gazedAtObject.CompareTag(InteractableTag)
            )
            {
                _gazedAtObject.SendMessage(
                    "OnPointerClickXR",
                    null,
                    SendMessageOptions.DontRequireReceiver
                );
            }
        }
    }


    // =========================================================
    // PUNTERO SOBRE OBJETO INTERACTUABLE
    // =========================================================

    private void PointerOnGaze(Vector3 point)
    {
        if (pointer == null)
            return;

        Transform pointerContainer = pointer.transform.parent;

        if (pointerContainer == null)
            return;


        // Adaptamos ligeramente el tamaño según la distancia.
        float scaleFactor =
            interactablePointerScale *
            Vector3.Distance(transform.position, point);

        pointer.transform.localScale =
            Vector3.one * scaleFactor;


        // Colocamos el puntero casi encima de la superficie.
        pointerContainer.position =
            CalculatePointerPosition(
                transform.position,
                point,
                disPointerObject
            );


        // Mantenemos la orientación relacionada con la cámara.
        pointerContainer.rotation = transform.rotation;
    }


    // =========================================================
    // PUNTERO NORMAL / IDLE
    // =========================================================

    private void PointerOutGaze()
    {
        if (pointer == null)
            return;

        Transform pointerContainer = pointer.transform.parent;

        if (pointerContainer == null)
            return;


        // Tamaño del punto central.
        pointer.transform.localScale =
            Vector3.one * idlePointerScale;


        // MUY IMPORTANTE:
        // coloca siempre el cursor directamente
        // delante de la cámara.
        pointerContainer.position =
            transform.position +
            transform.forward * maxDistancePointer;


        // Hace que el puntero siga la orientación
        // de la cabeza/cámara.
        pointerContainer.rotation =
            transform.rotation;


        // Apagamos únicamente el progreso de selección,
        // NO el punto central.
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.CancelGazeSelection();
        }
    }


    // =========================================================
    // INTERPOLACIÓN ENTRE CÁMARA Y OBJETO
    // =========================================================

    private Vector3 CalculatePointerPosition(
        Vector3 p0,
        Vector3 p1,
        float t
    )
    {
        return Vector3.Lerp(p0, p1, t);
    }
}