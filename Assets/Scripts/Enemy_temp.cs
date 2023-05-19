using System.Collections;
using UnityEngine;

public class Enemy_temp : MonoBehaviour
{
    [Header("Move Property")]
    [SerializeField] public GameObject[] movePoints;
    [HideInInspector] public IEnumerator coroutineMove;
    [HideInInspector] public IEnumerator coroutineAttack;

    private void Start()
    {
        Vector3[] movePointPos = new Vector3[movePoints.Length];
        for (int i = 0; i < movePoints.Length; i++)
        {
            movePointPos[i] = movePoints[i].transform.position
            + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        }
        EnemyMoving _enemyMoving = GetComponent<EnemyMoving>();
        coroutineMove = _enemyMoving.MoveToPoints(movePointPos);
        StartCoroutine(coroutineMove);

        EnemyAttacking _enemyAttacking = GetComponent<EnemyAttacking>();
        coroutineAttack = _enemyAttacking.Attack();
        StartCoroutine(coroutineAttack);
    }

}
