using System.Collections;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private GameObject player;
    private UnityEngine.AI.NavMeshAgent agent;

    public IEnumerator MoveToPoint(Vector3 movePointPos, float rotateSpeed)
    {
        player = GameObject.Find("Main Camera");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();     

        while (true)
        {
            agent.SetDestination(movePointPos);
            Vector3 distance = movePointPos - transform.position;

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
                    yield break;
                }
            }
            
            yield return null;
        }
    }
}
