using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[System.Serializable]
public class MovePoint
{
    [SerializeField] public GameObject[] _MovePoint;
}
public class Enemy_test : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject stonePrefab;
    [Space]
    [Header("Attack Property")]
    [SerializeField] private float attackInterval = 10f;
    [SerializeField] private float minimumAttackInterval = 5f;
    [Space]
    [Header("Move Property")]
    [SerializeField] private float moveInterval = 10f;
    [SerializeField] private float minimumMoveInterval = 5f;
    [SerializeField] private float moveLevelProbability = 0.3f;
    [SerializeField] private MovePoint[] movePointLevel;
    [SerializeField] private GameObject[] movePointLevel1;
    [SerializeField] private GameObject[] movePointLevel2;
    [SerializeField] private GameObject[] movePointLevel3;
    // [SerializeField] private Transform throwPoint;
    // [SerializeField] private int enemyDamage = 5;


    private float revisedAttackInterval;
    private float revisedMoveInterval;
    private int countMove = 0;
    private float timerThrow = 0f;
    private float timerMove = 0f;

    private float timeSinceLastThrow = 0f;
    private float timeSinceLastMove = 0f;
    private int currentLevel;
    private int currentPoint;
    private bool isMoving = false;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        revisedAttackInterval = attackInterval - minimumAttackInterval;
        revisedMoveInterval = moveInterval - minimumMoveInterval;
        currentPoint = Random.Range(0, movePointLevel.Length - 1);
        Debug.Log(movePointLevel[0]);
        isMoving = true;
        currentLevel = 1;
    }
    
    void Update()
    {
        // agent.SetDestination(player.transform.position);
        // Moving with Poisson process(expected time = moveInterval)
        agent.SetDestination(movePointLevel1[currentPoint].transform.position);
        if (agent.velocity.magnitude < 0.01) isMoving = false;
        else isMoving = true;
        Debug.Log(isMoving);


        if (isMoving == false) timeSinceLastMove += Time.deltaTime;
        if (timeSinceLastMove > minimumMoveInterval)
        {
            float p = Time.deltaTime / revisedMoveInterval; // probabilty for moving per 1 frame
            float randomNumber = Random.Range(0f, 1f / p);
            if (randomNumber > (1f / p) - 1)
            {
                if (Random.Range(0f, 1f) < moveLevelProbability)
                {

                }
                else
                {

                }
                randomNumber = Random.Range(0, 3);
                timeSinceLastMove = 0f;
            }
        }


        // Throwing with Poisson process(expected time = attackInterval)
        timeSinceLastThrow += Time.deltaTime;
        if (timeSinceLastThrow > minimumAttackInterval)
        {
            float p = Time.deltaTime / revisedAttackInterval; // probabilty for attacking per 1 frame
            float randomNumber = Random.Range(0f, 1f / p);
            if (randomNumber > (1f / p - 1) && isMoving == false)
            {
                // count += 1;
                // Debug.Log("count = " + count + ", mean = " + timer / count
                // + ", elapsed time = " + timeSinceLastThrow);
                ThrowStone();
                timeSinceLastThrow = 0f;
            }
        }

    }

    private void ThrowStone()
    {
        GameObject stone = Instantiate(stonePrefab, transform.position, transform.rotation);
    }
        
}
