using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField]
    private Transform enemySpawnArea;

    [SerializeField]
    private int maxLevel;

    [SerializeField]
    private int[] totalEnemyCount;

    [SerializeField]
    private float[] spawnInterval;

    [SerializeField]
    private GameObject[] enemyPrefabs;

    public int spawnedEnemyCount;
    public int leftEnemyCount;
    public int currentLevel;

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (currentLevel == 0)
        {
            currentLevel = 1;
        }
        LevelStart();
    }

    public void LevelStart()
    {
        float readyTime = 2f;

        spawnedEnemyCount = 0;
        leftEnemyCount = totalEnemyCount[currentLevel - 1];
        InvokeRepeating(nameof(SpawnEnemy), readyTime, spawnInterval[currentLevel - 1]);
    }

    private void SpawnEnemy()
    {
        float spawnAreaWidth = enemySpawnArea.localScale.x * 10;
        float spawnAreaHeight = enemySpawnArea.localScale.z * 10;

        float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
        float randomZ = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);

        Vector3 spawnPosition = enemySpawnArea.position + new Vector3(randomX, 0, randomZ);

        GameObject instance = Instantiate(
            enemyPrefabs[currentLevel - 1],
            spawnPosition,
            Quaternion.identity
        );

        float prefabHeight = instance.GetComponent<BoxCollider>().bounds.size.y;
        instance.transform.position += new Vector3(0, prefabHeight / 2, 0);

        spawnedEnemyCount += 1;

        if (spawnedEnemyCount >= totalEnemyCount[currentLevel - 1])
        {
            CancelInvoke(nameof(SpawnEnemy));
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
    }
}
