using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemySpaceShip : Enemy_temp
{
    [SerializeField]
    public Transform[] attackPoints;

    public IEnumerator MoveAndAttack()
    {
        while (true)
        {
            var targetPosition = attackPoints[Random.Range(0, attackPoints.Length)].position;
            transform.DOMove(targetPosition, 0.3f);
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
