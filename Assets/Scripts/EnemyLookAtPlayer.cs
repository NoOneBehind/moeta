using System.Collections;
using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] float rotationSpeed;

    public IEnumerator LookAtPlayer()
    { 
        while (true)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            Quaternion relativeRot = Quaternion.LookRotation(relativePos);
            Quaternion currentRot = transform.localRotation;
            transform.localRotation = Quaternion.RotateTowards(currentRot, relativeRot, rotationSpeed *Time.deltaTime);
            Quaternion diffRot = Quaternion.Inverse(relativeRot) * currentRot;
            Debug.Log(diffRot.y);

            if (diffRot.y < 0.001f) yield break;
            yield return null;
        }
    }

}
