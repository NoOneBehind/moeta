using System.Collections;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

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

    [SerializeField]
    private float rotateSpeed = 200f;
    private float revisedAttackInterval;
    private float timeSinceLastAttack = 0f;
    private bool isMoving = false;
    private UnityEngine.AI.NavMeshAgent agent;
    private IEnumerator _coroutineMove;

    private void start()
    {
        Enemy_temp _enemy_temp = GetComponent<Enemy_temp>();
        _coroutineMove = _enemy_temp.coroutineMove;
    }

    public IEnumerator Attack()
    {
        revisedAttackInterval = attackInterval - minimumAttackInterval;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        while (true)
        {
            if (agent.velocity.magnitude < 0.01f)
                isMoving = false;
            else
                isMoving = true;

            if (isMoving == false)
            {
                timeSinceLastAttack += Time.deltaTime;
                if (timeSinceLastAttack > minimumAttackInterval)
                {
                    float p = Time.deltaTime / revisedAttackInterval; // probabilty for attacking per 1 frame
                    float randomNumber = Random.Range(0f, 1f / p);
                    if (randomNumber > (1f / p - 1) && isMoving == false)
                    {
                        Debug.Log("Start throwing");
                        if (_coroutineMove != null)
                        {
                            StopCoroutine(_coroutineMove);
                            Debug.Log("Stop MoveToPoints");
                        }
                        yield return StartCoroutine(ThrowStone());
                        if (_coroutineMove != null)
                        {
                            StartCoroutine(_coroutineMove);
                        }
                        Debug.Log("Finished throwing");
                        timeSinceLastAttack = 0f;
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator ThrowStone()
    {
        Vector3 originPos = agent.transform.position;
        Vector3 throwPos =
            originPos + new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-2.0f, 2.0f));
        bool isThrown = false;

        // Move to throw position and throw
        while (isThrown == false)
        {
            // Debug.Log("Moving to throwing position : " + throwPos + originPos);

            agent.SetDestination(throwPos);
            Vector3 distance = transform.position - throwPos;
            if (agent.velocity.magnitude < 0.01f && distance.magnitude < 1.0f)
            {
                // Rotate enemy towards player
                Vector3 relativePos = player.transform.position - transform.position;
                Quaternion relativeRot = Quaternion.LookRotation(relativePos);
                Quaternion currentRot = transform.localRotation;
                transform.localRotation = Quaternion.RotateTowards(
                    currentRot,
                    relativeRot,
                    rotateSpeed * Time.deltaTime
                );
                Quaternion diffRot = Quaternion.Inverse(relativeRot) * currentRot;

                // Wait and throw stone when finished rotating
                if (Mathf.Abs(diffRot.y) < 0.01f)
                {
                    Debug.Log("Rotating Done");
                    yield return new WaitForSeconds(
                        Random.Range(waitBeforeThrowing - 1.0f, waitBeforeThrowing + 1.0f)
                    );
                    Debug.Log("Waiting for throwing Done");
                    GameObject stone = Instantiate(
                        stonePrefab,
                        transform.position,
                        transform.rotation
                    );
                    Stone _stone = stone.GetComponent<Stone>();
                    _stone.Throw(player.transform.position, 75f);
                    yield return new WaitForSeconds(
                        Random.Range(waitAfterThrowing - 1.0f, waitAfterThrowing + 1.0f)
                    );
                    Debug.Log("Waiting for returning done");
                    isThrown = true;
                }
            }
            yield return null;
        }

        // Move to original position
        while (isThrown == true)
        {
            agent.SetDestination(originPos);
            Vector3 distance = transform.position - originPos;
            if (agent.velocity.magnitude < 0.01f && distance.magnitude < 1.0f)
            {
                // Rotate enemy towards player
                Vector3 relativePos = player.transform.position - transform.position;
                Quaternion relativeRot = Quaternion.LookRotation(relativePos);
                Quaternion currentRot = transform.localRotation;
                transform.localRotation = Quaternion.RotateTowards(
                    currentRot,
                    relativeRot,
                    rotateSpeed * Time.deltaTime
                );
                Quaternion diffRot = Quaternion.Inverse(relativeRot) * currentRot;

                // Break coroutine
                if (Mathf.Abs(diffRot.y) < 0.01f)
                {
                    Debug.Log("Done throwing!");
                    yield break;
                }
            }
            yield return null;
        }
    }
}
