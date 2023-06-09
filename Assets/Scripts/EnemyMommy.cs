using System.Collections;
using UnityEngine;

public class EnemyMommy : Enemy_temp
{
    [Header("Mommy Attack Property")]
    [SerializeField]
    private GameObject mommyStonePrefab;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(MoveAndMommyAttack());
    }

    public IEnumerator MoveAndMommyAttack()
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
        yield return StartCoroutine(moving.MoveToPoint(movePointPos[0]));
        yield return StartCoroutine(moving.RotateTowards(player.transform.position, rotateSpeed));
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(minimumMoveInterval + Random.Range(0f, 2f));

        // // move to point2
        // animator.SetBool("isMoving", true);
        // yield return StartCoroutine(moving.MoveToPoint(movePointPos[1]));
        // yield return StartCoroutine(moving.RotateTowards(player.transform.position, rotateSpeed));
        // animator.SetBool("isMoving", false);
        // yield return new WaitForSeconds(minimumMoveInterval + Random.Range(0f, 2f));

        // // move to point3 (attack position)
        // animator.SetBool("isMoving", true);
        // yield return StartCoroutine(moving.MoveToPoint(movePointPos[2]));
        // yield return StartCoroutine(moving.RotateTowards(player.transform.position, rotateSpeed));
        // animator.SetBool("isMoving", false);

        // attack
        while (true)
        {
            attacking.MommyAttack(mommyStonePrefab, Random.Range(30f, 60f));
            yield return new WaitForSeconds(minimumAttackInterval + Random.Range(0f, 3f));
        }
    }
}
