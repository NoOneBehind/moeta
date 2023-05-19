using System.Collections;
using UnityEngine;

public class Enemy_temp : MonoBehaviour
{
    private GameObject player;

    [Header("Move Property")]
    [SerializeField]
    private GameObject[] movePoints;
    private Vector3[] movePointPos;
    [SerializeField]
    private float moveInterval = 10f;
    [SerializeField]
    private float minimumMoveInterval = 5f;
    [SerializeField]
    private float rotateSpeed = 200f;
    
    private float revisedMoveInterval;
    private float timeSinceLastMove = 0f;
    private int currentPoint = 0;
    private bool isMovingNext = true;

    [Space (10f)]
    [Header("Attack Property")]
    [SerializeField]
    private GameObject stonePrefab;
    [SerializeField]
    private float attackInterval = 10.0f;

    [SerializeField]
    private float minimumAttackInterval = 5.0f;

    [SerializeField]
    private float waitBeforeThrowing = 2.0f;

    [SerializeField]
    private float waitAfterThrowing = 2.0f;

    private float revisedAttackInterval;
    private float timeSinceLastAttack = 0f;

    private UnityEngine.AI.NavMeshAgent agent;

    private IEnumerator coroutineMove;
    private IEnumerator coroutineAttack;

    private void Start()
    {
        player = GameObject.Find("Main Camera");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        revisedMoveInterval = moveInterval - minimumMoveInterval;
        revisedAttackInterval = attackInterval - minimumAttackInterval;

        // Set move positions with random noise
        movePointPos = new Vector3[movePoints.Length];
        for (int i = 0; i < movePoints.Length; i++)
        {
            movePointPos[i] = movePoints[i].transform.position
            + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        }

        StartCoroutine(MoveAndAttack());
    }

    private IEnumerator MoveAndAttack()
    {
        while(true)
        {
            if (isMovingNext)
            {
                Debug.Log("Moving to next position");
                coroutineMove = GetComponent<Moving>().MoveToPoint(movePointPos[currentPoint], rotateSpeed);
                yield return StartCoroutine(coroutineMove);
                isMovingNext = false;
            }

            // Move to next position
            timeSinceLastMove += Time.deltaTime;
            if ((timeSinceLastMove > minimumMoveInterval) && (currentPoint != movePointPos.Length - 1))
            {
                float p = Time.deltaTime / revisedMoveInterval; // probabilty for moving per 1 frame
                float randomNumber = Random.Range(0f, 1f / p);
                if (randomNumber > (1f / p) - 1)
                {
                    currentPoint += 1;
                    isMovingNext = true;
                    timeSinceLastMove = 0f;
                }
            }

            // Attack
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack > minimumAttackInterval)
            {
                float p = Time.deltaTime / revisedAttackInterval; // probabilty for attacking per 1 frame
                float randomNumber = Random.Range(0f, 1f / p);
                if (randomNumber > (1f / p - 1))
                {
                    Vector3 originPos = agent.transform.position;
                    Vector3 throwPos =
                        originPos + new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-2.0f, 2.0f));
                    
                    // Move to throw position
                    Debug.Log("Start moving to throw");
                    coroutineMove = GetComponent<Moving>().MoveToPoint(throwPos, rotateSpeed);
                    yield return StartCoroutine(coroutineMove);

                    // Wait to throw
                    Debug.Log("Waiting for throwing");
                    yield return new WaitForSeconds(
                        Random.Range(waitBeforeThrowing - 1.0f, waitBeforeThrowing + 1.0f));

                    // Throw
                    Attacking _enemyAttacking = GetComponent<Attacking>();
                    coroutineAttack = _enemyAttacking.Attack(stonePrefab, Random.Range(30f, 60f));
                    yield return StartCoroutine(coroutineAttack);

                    // Wait to return
                    Debug.Log("Waiting for returning");
                    yield return new WaitForSeconds(
                        Random.Range(waitAfterThrowing - 1.0f, waitAfterThrowing + 1.0f));
                    
                    // Returning to original position
                    Debug.Log("Start returning to original position");
                    coroutineMove = GetComponent<Moving>().MoveToPoint(originPos, rotateSpeed);
                    yield return StartCoroutine(coroutineMove);

                    Debug.Log("Finished throwing");
                    timeSinceLastAttack = 0f;
                }
            }

            yield return null;
        }
    }

}
