using System.Collections;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private GameObject player;
    private UnityEngine.AI.NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public IEnumerator MoveToPoint(Vector3 movePointPos)
    {
        while (true)
        {
            agent.SetDestination(movePointPos);
            Vector3 distance = movePointPos - transform.position;

            if (distance.magnitude <= 5f && agent.velocity.magnitude <= 0.01f)
            {
                Debug.Log("done moving");
                yield break;
            }

            yield return null;
        }
    }

    public IEnumerator RotateTowards(Vector3 targetPosition, float rotateSpeed)
    {
        while (true)
        {
            Vector3 relativePos = targetPosition - transform.position;
            Quaternion relativeRot = Quaternion.LookRotation(relativePos);
            Quaternion currentRot = transform.localRotation;
            transform.localRotation = Quaternion.RotateTowards(
                currentRot,
                relativeRot,
                rotateSpeed * Time.deltaTime
            );
            Quaternion diffRot = Quaternion.Inverse(relativeRot) * currentRot;

            // When rotating is done
            if (Mathf.Abs(diffRot.y) < 0.01f)
            {
                Debug.Log("rotating done");
                yield break;
            }

            yield return null;
        }
    }
}
