using UnityEngine;

public class GrabObject : MonoBehaviour
{
    private GrabManager grabManager;
    private BoxCollider boxCollider;

    private Vector3 spawnerPosition;
    private Quaternion spawnerRotation;

    [Header("Configuración")]
    [SerializeField]
    public string type = "Objeto";

    [SerializeField]
    public GameObject spawner;


    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

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
                "GrabObject: No se encontró GrabManager en la escena."
            );
        }


        if (spawner != null)
        {
            spawnerPosition =
                spawner.transform.position;

            spawnerRotation =
                spawner.transform.rotation;
        }
        else
        {
            // Si no se asignó spawner,
            // usamos la posición inicial del objeto.
            spawnerPosition =
                transform.position;

            spawnerRotation =
                transform.rotation;
        }
    }


    // =====================================================
    // AGARRAR
    // =====================================================

    public void Grab()
    {
        if (grabManager == null)
            return;


        // Si ya estamos sosteniendo otro objeto,
        // lo devolvemos primero.
        if (grabManager.heldItem != null)
        {
            GrabObject previousObject =
                grabManager.heldItem
                    .GetComponent<GrabObject>();

            if (previousObject != null)
            {
                previousObject.Drop();
            }
        }


        grabManager.heldItem =
            gameObject;


        // Desactivamos el collider mientras
        // estamos sosteniendo el objeto.
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }


    // =====================================================
    // SOLTAR / DEVOLVER A SU POSICIÓN ORIGINAL
    // =====================================================

    public void Drop()
    {
        transform.position =
            spawnerPosition;

        transform.rotation =
            spawnerRotation;


        if (grabManager != null)
        {
            grabManager.heldItem = null;
        }


        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }


    // =====================================================
    // ELIMINAR / OCULTAR OBJETO
    // =====================================================

    public void Delete()
    {
        transform.position =
            spawnerPosition;

        transform.rotation =
            spawnerRotation;


        if (grabManager != null)
        {
            grabManager.heldItem = null;
        }


        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }


        gameObject.SetActive(false);
    }


    // =====================================================
    // REAPARECER
    // =====================================================

    public void Respawn()
    {
        transform.position =
            spawnerPosition;

        transform.rotation =
            spawnerRotation;


        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }


        gameObject.SetActive(true);
    }


    // =====================================================
    // COLOCAR OBJETO
    // =====================================================

    public void Place(Vector3 position)
    {
        transform.position =
            position;


        if (grabManager != null)
        {
            grabManager.heldItem = null;
        }


        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }


    // =====================================================
    // SELECCIÓN MEDIANTE CARDBOARD
    // =====================================================

    public void OnPointerClickXR()
    {
        Grab();
    }
}