using UnityEngine;

public class FinalMessageAnimation : MonoBehaviour
{
    [SerializeField]
    private float animationSpeed = 4f;

    private Vector3 finalScale;

    private void Start()
    {
        finalScale = transform.localScale;

        transform.localScale =
            Vector3.zero;
    }

    private void Update()
    {
        transform.localScale =
            Vector3.Lerp(
                transform.localScale,
                finalScale,
                Time.deltaTime * animationSpeed
            );
    }
}