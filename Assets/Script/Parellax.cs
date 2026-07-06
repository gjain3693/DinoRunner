using UnityEngine;

public class Parellax : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    [SerializeField]
    private float animationSpeed = 0.5f;

    private void Awake()
    {
        meshRenderer =
            GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogError(
                "Parellax: MeshRenderer missing on " +
                gameObject.name
            );
        }
    }

    private void Update()
    {
        if (meshRenderer == null)
        {
            return;
        }

        if (meshRenderer.material == null)
        {
            return;
        }

        meshRenderer.material.mainTextureOffset +=
            new Vector2(
                animationSpeed * Time.deltaTime,
                0f
            );
    }

    private void OnValidate()
    {
        if (animationSpeed < 0f)
        {
            animationSpeed = 0f;
        }
    }
}