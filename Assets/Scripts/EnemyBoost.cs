using System.Collections;
using UnityEngine;

public class EnemyBoost : Enemy_temp
{
    [Header("Boost Attack Property")]
    [SerializeField]
    private GameObject boostStonePrefab;

    public IEnumerator MoveAndBoostAttack()
    {
        movePointPos = new Vector3[movePoints.Length];
        for (int i = 0; i < movePoints.Length; i++)
        {
            movePointPos[i] =
                movePoints[i].transform.position
                + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        }

        // move to point1
        animator.SetBool("isMoving", true);
        yield return StartCoroutine(moving.MoveToPoint(movePointPos[1]));
        yield return StartCoroutine(moving.RotateTowards(player.transform.position, rotateSpeed));
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(minimumMoveInterval + Random.Range(0f, 2f));

        // attack
        while (true)
        {
            attacking.BoostAttack(boostStonePrefab, Random.Range(30f, 60f));
            yield return new WaitForSeconds(minimumAttackInterval + Random.Range(0f, 3f));
        }
    }
}
