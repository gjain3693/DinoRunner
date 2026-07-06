using UnityEngine;

public class AlternatingSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    public float spawnRate = 2f;

    public float verticalOffset = 1.5f;

    private int currentIndex = 0;

    private bool skipNextSpawn = false;

    private void OnEnable()
    {
        InvokeRepeating(
            nameof(Spawn),
            spawnRate,
            spawnRate
        );
    }

    private void Spawn()
    {
        if (skipNextSpawn)
        {
            skipNextSpawn = false;
            return;
        }

        if (obstaclePrefabs == null ||
            obstaclePrefabs.Length == 0)
        {
            return;
        }

        GameObject prefab =
            obstaclePrefabs[currentIndex];

        Vector3 spawnPosition =
            transform.position +
            Vector3.up *
            Random.Range(0f, verticalOffset);

        Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );

        currentIndex =
            (currentIndex + 1) %
            obstaclePrefabs.Length;
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    public void ClearObstacles()
    {
        GameObject[] obstacles =
            GameObject.FindGameObjectsWithTag(
                "Obstacle"
            );

        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
    }

    public void SkipNext()
    {
        skipNextSpawn = true;
    }
}