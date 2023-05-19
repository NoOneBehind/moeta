using System.Collections;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3[] movePointPos;
    [SerializeField] private float moveInterval = 10f;
    [SerializeField] private float minimumMoveInterval = 5f;
    [SerializeField] private float rotateSpeed = 200f;
    private float revisedMoveInterval;
    private float timeSinceLastMove = 0f;
    private int currentPoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;

    public static EnemyMoving CreateEnemyMoving (GameObject where, GameObject _player,
    float _moveInterval = 10f, float _minimumMoveInterval = 5f)
    {
        EnemyMoving _enemyMoving = where.AddComponent<EnemyMoving>();
        _enemyMoving.player = _player;
        _enemyMoving.moveInterval = _moveInterval;
        _enemyMoving.minimumMoveInterval = _minimumMoveInterval;
        return _enemyMoving;
    }

    public IEnumerator MoveToPoints(Vector3[] movePointPos)
    {
        revisedMoveInterval = moveInterval - minimumMoveInterval;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();     

        while (true)
        {
            agent.SetDestination(movePointPos[currentPoint]);
            Vector3 distance = movePointPos[currentPoint] - transform.position;

            // When reached the destination
            if (agent.velocity.magnitude < 0.01f && distance.magnitude < 1.0f)
            {
                // Rotate enemy towards player
                Vector3 relativePos = player.transform.position - transform.position;
                Quaternion relativeRot = Quaternion.LookRotation(relativePos);
                Quaternion currentRot = transform.localRotation;
                transform.localRotation = Quaternion.RotateTowards(currentRot, relativeRot, rotateSpeed * Time.deltaTime);
                Quaternion diffRot = Quaternion.Inverse(relativeRot) * currentRot;

                // When rotating is done
                if (Mathf.Abs(diffRot.y) < 0.01f)
                {
                    if (currentPoint == movePointPos.Length - 1) { Debug.Log("Breaked"); yield break; }
                    timeSinceLastMove += Time.deltaTime;
                    if (timeSinceLastMove > minimumMoveInterval)
                    {
                        float p = Time.deltaTime / revisedMoveInterval; // probabilty for moving per 1 frame
                        float randomNumber = Random.Range(0f, 1f / p);
                        if (randomNumber > (1f / p) - 1)
                        {
                            currentPoint += 1;
                            timeSinceLastMove = 0f;
                        }
                    }
                }
            }
            
            yield return null;
        }
    }
}
