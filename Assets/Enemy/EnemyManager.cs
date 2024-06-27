using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float minSpawnDistanceFromPlayer;

    void Update()
    {
        EnsureOneEnemy();
    }

    void EnsureOneEnemy()
    {
        if (FindObjectsOfType<Enemy>().Length == 0)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector2 position = GetRandomPosition();
        Instantiate(enemyPrefab, position, transform.rotation);
    }

    Vector2 GetRandomPosition()
    {
        Vector2 randomPosition;
        Camera mainCamera = Camera.main;
        Vector2 cameraMin = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 cameraMax = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        do
        {
            randomPosition = new Vector2(
                Random.Range(cameraMin.x, cameraMax.x),
                Random.Range(cameraMin.y, cameraMax.y)
            );
        }
        while (Vector2.Distance(randomPosition, GameObject.FindGameObjectWithTag("Player").transform.position) < minSpawnDistanceFromPlayer);

        return randomPosition;
    }
}
