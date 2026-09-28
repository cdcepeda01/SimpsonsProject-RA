using UnityEngine;

public class FloatingDonut : MonoBehaviour
{
    [Header("Levitación")]
    [SerializeField] private float floatHeight = 0.15f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Rotación")]
    [SerializeField] private float rotationSpeed = 45f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        // =========================
        // LEVITACIÓN
        // =========================

        float newY =
            Mathf.Sin(Time.time * floatSpeed) *
            floatHeight;

        transform.localPosition =
            startPosition +
            Vector3.up * newY;


        // =========================
        // ROTACIÓN
        // =========================

        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime,
            Space.Self
        );
    }
}