using UnityEngine;

public class Ninja : MonoBehaviour
{
    private float leftEdge;

    [SerializeField]
    private float speed = 5f;

    private void Start()
    {
        if (Camera.main != null)
        {
            leftEdge =
                Camera.main
                .ScreenToWorldPoint(Vector3.zero)
                .x - 1f;
        }
        else
        {
            Debug.LogError(
                "Ninja: Main Camera not found."
            );
        }
    }

    private void Update()
    {
        transform.position +=
            Vector3.left *
            speed *
            Time.deltaTime;

        if (transform.position.x < leftEdge)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnValidate()
    {
        if (speed < 0f)
        {
            speed = 0f;
        }
    }
}