using UnityEngine;
using System.Collections;

public class DualSpawnner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    public float spawnRate = 3f;

    private bool skipNextSpawn = false;

    private void OnEnable()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnPair());

            yield return new WaitForSeconds(spawnRate);
        }
    }

    private IEnumerator SpawnPair()
    {
        if (skipNextSpawn)
        {
            skipNextSpawn = false;
            yield break;
        }

        if (obstaclePrefabs == null ||
            obstaclePrefabs.Length < 2)
        {
            yield break;
        }

        int firstIndex =
            Random.Range(0, obstaclePrefabs.Length);

        GameObject firstPrefab =
            obstaclePrefabs[firstIndex];

        if (firstPrefab == null)
        {
            yield break;
        }

        Vector3 firstPos =
            transform.position;

        GameObject firstObj =
            Instantiate(
                firstPrefab,
                firstPos,
                Quaternion.identity
            );

        SpriteRenderer sr1 =
            firstObj.GetComponent<SpriteRenderer>();

        float firstHeight = 2f;

        if (sr1 != null)
        {
            firstHeight = sr1.bounds.size.y;
        }

        int secondIndex =
            Random.Range(0, obstaclePrefabs.Length);

        GameObject secondPrefab =
            obstaclePrefabs[secondIndex];

        if (secondPrefab != null)
        {
            Vector3 secondPos =
                firstPos +
                Vector3.up * firstHeight;

            Instantiate(
                secondPrefab,
                secondPos,
                Quaternion.identity
            );
        }

        yield return null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
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

    // Continue button support
    public void RestartSpawner()
    {
        StopAllCoroutines();

        StartCoroutine(SpawnLoop());
    }

    private void OnValidate()
    {
        if (spawnRate < 0.1f)
        {
            spawnRate = 0.1f;
        }
    }
}