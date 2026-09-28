using System.Collections.Generic;
using UnityEngine;

public class GrabManager : MonoBehaviour
{
    [Header("Objetos")]
    public GameObject[] interactables;
    public List<GameObject> objects = new List<GameObject>();
    public List<GameObject> products = new List<GameObject>();

    [HideInInspector]
    public GameObject heldItem;


    [Header("Posición del objeto agarrado")]

    [Tooltip("Normalmente debe ser la Main Camera.")]
    [SerializeField]
    private Transform holdReference;

    [Tooltip("Distancia a la que se mantiene el objeto frente al jugador.")]
    [SerializeField]
    private float holdDistance = 1.5f;

    [Tooltip("Desplazamiento vertical del objeto.")]
    [SerializeField]
    private float verticalOffset = -0.15f;

    [Tooltip("Velocidad con la que el objeto sigue la mirada.")]
    [SerializeField]
    private float followSpeed = 12f;


    private void Start()
    {
        // Si no asignamos cámara manualmente,
        // usamos la cámara principal.
        if (holdReference == null && Camera.main != null)
        {
            holdReference = Camera.main.transform;
        }


        // Conservamos esta parte por compatibilidad
        // con el proyecto original.
        interactables =
            GameObject.FindGameObjectsWithTag("Interactable");

        foreach (GameObject item in interactables)
        {
            GrabObject grabObject =
                item.GetComponent<GrabObject>();

            if (grabObject != null)
            {
                objects.Add(item);

                if (grabObject.type != "Objeto")
                {
                    products.Add(item);
                }
            }
        }
    }


    private void LateUpdate()
    {
        if (heldItem == null)
            return;

        if (holdReference == null)
            return;


        // Punto delante de donde está mirando el jugador.
        Vector3 targetPosition =
            holdReference.position
            + holdReference.forward * holdDistance
            + holdReference.up * verticalOffset;


        // Seguimiento suave.
        heldItem.transform.position =
            Vector3.Lerp(
                heldItem.transform.position,
                targetPosition,
                1f - Mathf.Exp(
                    -followSpeed * Time.deltaTime
                )
            );
    }
}