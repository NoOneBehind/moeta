using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemySpaceship : Enemy_temp
{
    [SerializeField]
    public Transform[] attackPoints;

    [SerializeField]
    private GameObject mommyStonePrefab;

    [SerializeField]
    private GameObject boostStonePrefab;

    public IEnumerator MoveAndAttack()
    {
        while (true)
        {
            var targetPosition = attackPoints[Random.Range(0, attackPoints.Length)].position;
            var randomSeed = Random.Range(0f, 10f);
            DOTween
                .Sequence()
                .Append(transform.DOMove(targetPosition, 0.3f))
                .AppendCallback(() =>
                {
                    if (randomSeed > 5)
                    {
                        attacking.BoostAttack(boostStonePrefab, Random.Range(5f, 10f), true);
                    }
                    else
                    {
                        attacking.MommyAttack(mommyStonePrefab, Random.Range(5f, 10f), true);
                    }
                });
            yield return new WaitForSeconds(minimumMoveInterval + Random.Range(0f, 2f));
        }
    }

    private void Update()
    {
        Vector3 targetPosition = player.transform.position;
        transform.LookAt(targetPosition);

        Vector3 currentAngles = transform.rotation.eulerAngles;

        currentAngles.x -= 90;
        currentAngles.z += 90;

        transform.rotation = Quaternion.Euler(currentAngles);
    }
}
