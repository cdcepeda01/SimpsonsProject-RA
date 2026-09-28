using UnityEngine;
using UnityEngine.Events;

public class DuffPlatform : MonoBehaviour
{
    [Header("Objeto requerido")]

    [Tooltip("Objeto que esta plataforma acepta.")]
    [SerializeField]
    private GameObject requiredObject;


    [Header("Punto de colocación")]

    [Tooltip("Lugar exacto donde quedará colocado el objeto.")]
    [SerializeField]
    private Transform placementPoint;


    [Header("Tiempo de selección")]

    [Tooltip("Tiempo necesario mirando la plataforma para colocar el objeto.")]
    [SerializeField]
    private float platformGazeTime = 0.8f;

    [Tooltip("Tiempo normal del sistema de mirada.")]
    [SerializeField]
    private float defaultGazeTime = 2.5f;


    [Header("Eventos")]

    public UnityEvent OnDuffPlaced;


    private GrabManager grabManager;
    private Collider platformCollider;

    private bool completed = false;


    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        platformCollider =
            GetComponent<Collider>();


        GameObject managerObject =
            GameObject.Find("GrabManager");


        if (managerObject != null)
        {
            grabManager =
                managerObject.GetComponent<GrabManager>();
        }


        if (grabManager == null)
        {
            Debug.LogError(
                "DuffPlatform: No se encontró GrabManager en la escena."
            );
        }


        if (placementPoint == null)
        {
            Debug.LogWarning(
                "DuffPlatform: No se asignó PlacementPoint."
            );
        }
    }


    // =====================================================
    // EMPIEZA A MIRAR LA PLATAFORMA
    // =====================================================

    public void OnPointerEnterXR()
    {
        if (completed)
            return;


        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(
                platformGazeTime
            );
        }
    }


    // =====================================================
    // DEJA DE MIRAR LA PLATAFORMA
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
    // SELECCIONA LA PLATAFORMA
    // =====================================================

    public void OnPointerClickXR()
    {
        if (completed)
            return;


        if (grabManager == null)
            return;


        // ¿El jugador lleva algo?
        GameObject heldItem =
            grabManager.heldItem;


        if (heldItem == null)
        {
            Debug.Log(
                "DuffPlatform: El jugador no está sosteniendo ningún objeto."
            );

            return;
        }


        // ¿Es el objeto correcto?
        if (
            requiredObject != null &&
            heldItem != requiredObject
        )
        {
            Debug.Log(
                "DuffPlatform: Ese objeto no corresponde a esta plataforma."
            );

            return;
        }


        PlaceObject(heldItem);
    }


    // =====================================================
    // COLOCAR OBJETO
    // =====================================================

    private void PlaceObject(GameObject objectToPlace)
    {
        GrabObject grabObject =
            objectToPlace.GetComponent<GrabObject>();


        if (grabObject == null)
        {
            Debug.LogError(
                "DuffPlatform: El objeto no tiene GrabObject."
            );

            return;
        }


        Vector3 targetPosition;

        if (placementPoint != null)
        {
            targetPosition =
                placementPoint.position;
        }
        else
        {
            targetPosition =
                transform.position;
        }


        // Utilizamos el sistema de colocación
        // del GrabObject.
        grabObject.Place(
            targetPosition
        );


        // Ajustamos también la rotación.
        if (placementPoint != null)
        {
            objectToPlace.transform.rotation =
                placementPoint.rotation;
        }


        completed = true;


        // Ya no necesitamos interactuar otra vez
        // con la plataforma.
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }


        // Restauramos tiempo normal.
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(
                defaultGazeTime
            );
        }


        Debug.Log(
            "Duff colocada correctamente."
        );


        // Avisamos que el objetivo fue completado.
        OnDuffPlaced?.Invoke();
    }
}