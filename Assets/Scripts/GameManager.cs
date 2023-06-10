using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField]
    private Transform enemySpawnArea;

    [SerializeField]
    private Transform[] spaceshipeMovePoints;

    [SerializeField]
    private int maxLevel;

    [SerializeField]
    private int[] totalEnemyCount;

    [SerializeField]
    private float[] spawnInterval;

    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private GameObject[] enemyMovePointsLevel1;

    [SerializeField]
    private GameObject[] enemyMovePointsLevel2;

    [SerializeField]
    private GameObject[] enemyMovePointsLevel3;

    [SerializeField]
    private TextMeshProUGUI gameStateNoticeText;

    public int spawnedEnemyCount;
    public int leftEnemyCount;
    public int deadEnemyCount;
    public int currentLevel;

    private Dictionary<GameObject, bool> visitedPointsLevel3 = new Dictionary<GameObject, bool>();

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
        foreach (var point in enemyMovePointsLevel3)
        {
            visitedPointsLevel3[point] = false;
        }

        float readyTime = 6f;

        gameStateNoticeText.text = "Level " + currentLevel.ToString();
        Sequence mySquence = DOTween.Sequence();

        if (currentLevel == 1)
        {
            mySquence.Append(
                enemySpawnArea.DOPath(
                    spaceshipeMovePoints.Select(point => point.position).ToArray(),
                    4,
                    PathType.CatmullRom
                )
            );
        }

        mySquence
            .Append(gameStateNoticeText.DOColor(new Color(1, 1, 1, 0), 0))
            .Append(gameStateNoticeText.DOFade(1f, readyTime / 2).SetEase(Ease.InQuart))
            .AppendInterval(readyTime / 2)
            .Append(gameStateNoticeText.rectTransform.DOLocalMoveZ(-400f, 2f).SetEase(Ease.InQuart))
            .Append(gameStateNoticeText.DOFade(0f, 0))
            .Join(gameStateNoticeText.rectTransform.DOLocalMoveZ(0, 0));

        spawnedEnemyCount = 0;
        deadEnemyCount = 0;
        leftEnemyCount = totalEnemyCount[currentLevel - 1];

        InvokeRepeating(nameof(SpawnEnemy), readyTime, spawnInterval[currentLevel - 1]);
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = enemySpawnArea.position;

        GameObject enemyInstance = Instantiate(
            enemyPrefabs[currentLevel - 1],
            spawnPosition,
            Quaternion.Euler(0, 180, 0)
        );
        NavMeshAgent agent = enemyInstance.GetComponent<NavMeshAgent>();

        Sequence mySquence = DOTween
            .Sequence()
            .Append(enemyInstance.gameObject.transform.DOMoveY(9f, 3f))
            .Join(
                enemyInstance.gameObject.transform
                    .DORotate(new Vector3(0, 360 * 2 + 180, 0), 3f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
            )
            .OnComplete(() => InitEnemyInstance(enemyInstance));

        spawnedEnemyCount += 1;

        if (spawnedEnemyCount >= totalEnemyCount[currentLevel - 1])
        {
            CancelInvoke(nameof(SpawnEnemy));
        }
    }

    private void InitEnemyInstance(GameObject enemyInstance)
    {
        NavMeshAgent agent = enemyInstance.GetComponent<NavMeshAgent>();
        Enemy_temp enemy = enemyInstance.GetComponent<Enemy_temp>();

        enemy.movePoints.SetValue(GetRandomElement(enemyMovePointsLevel1), 0);
        enemy.movePoints.SetValue(GetRandomElement(enemyMovePointsLevel2), 1);

        var unvisitedPoint = GetRandomUnvisitedElement(enemyMovePointsLevel3, visitedPointsLevel3);
        enemy.movePoints.SetValue(unvisitedPoint, 2);
        visitedPointsLevel3[unvisitedPoint] = true;

        enemy.SetHealthBar();
        agent.enabled = true;
        StartCoroutine(enemy.MoveAndAttackLevel1());
    }

    public void GameOver()
    {
        CancelInvoke(nameof(SpawnEnemy));

        StopAllCoroutines();

        gameStateNoticeText.text = "Game Over";
        Sequence mySquence = DOTween
            .Sequence()
            .Append(gameStateNoticeText.DOColor(new Color(1, 0, 0, 0), 0))
            .Append(gameStateNoticeText.DOFade(1f, 2f).SetEase(Ease.InQuart));
    }

    private static T GetRandomElement<T>(T[] array)
    {
        int randomIndex = Random.Range(0, array.Length);
        return array[randomIndex];
    }

    private static T GetRandomUnvisitedElement<T>(T[] array, Dictionary<T, bool> visitedMap)
    {
        var unvisitedElements = array.Where(element => !visitedMap[element]).ToArray();
        if (unvisitedElements.Length == 0)
        {
            foreach (var element in array)
            {
                visitedMap[element] = false;
            }
            unvisitedElements = array;
        }

        int randomIndex = Random.Range(0, unvisitedElements.Length);
        return unvisitedElements[randomIndex];
    }
}
