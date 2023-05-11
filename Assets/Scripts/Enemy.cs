using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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
    [SerializeField] private GameObject[] movePoint;
    // [SerializeField] private float moveLevelProbability = 0.3f;
    // [SerializeField] private Transform throwPoint;
    // [SerializeField] private int enemyDamage = 5;


    private float revisedAttackInterval;
    private float revisedMoveInterval;
    private float timeSinceLastThrow = 0f;
    private float timeSinceLastMove = 0f;
    private int currentPoint;
    private bool isMoving = false;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        revisedAttackInterval = attackInterval - minimumAttackInterval;
        revisedMoveInterval = moveInterval - minimumMoveInterval;
        currentPoint = Random.Range(0, movePoint.Length);
        isMoving = true;
    }
    
    void Update()
    {
        // Moving with Poisson process(expected time = moveInterval)
        agent.SetDestination(movePoint[currentPoint].transform.position);
        if (agent.velocity.magnitude < 0.01f)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            Quaternion relativeRot = Quaternion.LookRotation(relativePos);
            Quaternion currentRot = transform.localRotation;
            transform.localRotation = Quaternion.Slerp(currentRot, relativeRot, Time.deltaTime * 5);
            isMoving = false;
        }        
        else
        {
            isMoving = true;
        }
        Debug.Log(isMoving);


        if (isMoving == false)
        {
            timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove > minimumMoveInterval)
            {
                float p = Time.deltaTime / revisedMoveInterval; // probabilty for moving per 1 frame
                float randomMove = Random.Range(0f, 1f / p);
                if (randomMove > (1f / p) - 1)
                {
                    currentPoint = Random.Range(0, movePoint.Length);
                    timeSinceLastMove = 0f;
                }
            }
        }


        // Throwing with Poisson process(expected time = attackInterval)
        timeSinceLastThrow += Time.deltaTime;
        if (timeSinceLastThrow > minimumAttackInterval)
        {
            float p = Time.deltaTime / revisedAttackInterval; // probabilty for attacking per 1 frame
            float randomThrow = Random.Range(0f, 1f / p);
            if (randomThrow > (1f / p - 1) && isMoving == false)
            {
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
