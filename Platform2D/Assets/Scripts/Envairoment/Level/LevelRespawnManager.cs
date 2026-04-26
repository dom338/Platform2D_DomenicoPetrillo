using System.Collections.Generic;
using UnityEngine;

public class LevelRespawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public Transform spawnPoint;
    }

    [SerializeField] private List<EnemySpawnData> enemiesToSpawn = new List<EnemySpawnData>();

    private List<GameObject> currentEnemies = new List<GameObject>();

    private void Start()
    {
        RespawnEnemies();
    }

    public void RespawnEnemies()
    {
        foreach (GameObject enemy in currentEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        currentEnemies.Clear();

        foreach (EnemySpawnData data in enemiesToSpawn)
        {
            if (data.enemyPrefab != null && data.spawnPoint != null)
            {
                GameObject enemy = Instantiate(
                    data.enemyPrefab,
                    data.spawnPoint.position,
                    data.spawnPoint.rotation
                );

                currentEnemies.Add(enemy);
            }
        }
    }
}