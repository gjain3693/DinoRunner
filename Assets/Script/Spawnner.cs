using UnityEngine;

public class Spawnner : MonoBehaviour
{
    public GameObject ninjaPrefab;

    private bool skipNextSpawn = false;

    public float spawnRate = 1f;

    private void OnEnable()
    {
        InvokeRepeating(
            nameof(Spawn),
            spawnRate,
            spawnRate
        );
    }

    public void Spawn()
    {
        if (skipNextSpawn)
        {
            skipNextSpawn = false;
            return;
        }

        if (ninjaPrefab == null)
        {
            Debug.LogWarning(
                "Spawnner: Ninja Prefab is not assigned."
            );
            return;
        }

        GameObject ninja =
            Instantiate(
                ninjaPrefab,
                transform.position,
                Quaternion.identity
            );

        if (ninja != null)
        {
            ninja.transform.position +=
                Vector3.up *
                Random.Range(-1f, 1f);
        }
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }

    public void ClearObstacles()
    {
        GameObject[] obstacles =
            GameObject.FindGameObjectsWithTag(
                "Obstacle"
            );

        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
            {
                Destroy(obstacle);
            }
        }
    }

    public void SkipNext()
    {
        skipNextSpawn = true;
    }

    private void OnValidate()
    {
        if (spawnRate < 0.1f)
        {
            spawnRate = 0.1f;
        }
    }
}