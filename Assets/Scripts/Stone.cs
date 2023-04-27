using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Vector3 targetPostion;
    void Start()
    {
        GameObject mainCameraObj = GameObject.FindWithTag("MainCamera");
        targetPostion = mainCameraObj.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0, 0);

        Vector3 v = CalculateInitialVelocity(transform.position, targetPostion, 45f);
        GetComponent<Rigidbody>().velocity = v;
    }

    void Update()
    { }


    public static Vector3 CalculateInitialVelocity(Vector3 startPoint, Vector3 endPoint, float angle, float gravity = 9.81f) {
        Vector3 dir = endPoint - startPoint;
        float height = dir.y;
        dir.y = 0;
        float dist = dir.magnitude;
        float a = angle * Mathf.Deg2Rad;
        dir.y = dist * Mathf.Tan(a);
        dist += height / Mathf.Tan(a);

        float vel = Mathf.Sqrt(dist * gravity / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }
}

