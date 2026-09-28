using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Levitación")]
    [SerializeField] private float floatHeight = 0.08f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Rotación")]
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed = 25f;

    private Vector3 initialLocalPosition;

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        // ==============================
        // LEVITACIÓN
        // ==============================

        float verticalOffset =
            Mathf.Sin(Time.time * floatSpeed) *
            floatHeight;

        transform.localPosition =
            initialLocalPosition +
            Vector3.up * verticalOffset;


        // ==============================
        // ROTACIÓN
        // ==============================

        if (rotate)
        {
            transform.Rotate(
                Vector3.up,
                rotationSpeed * Time.deltaTime,
                Space.Self
            );
        }
    }
}