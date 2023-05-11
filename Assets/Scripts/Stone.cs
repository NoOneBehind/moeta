using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stone : MonoBehaviour
{
    private Vector3 targetPostion;

    void Start()
    {
        GameObject mainCameraObj = GameObject.FindWithTag("MainCamera");
        Rigidbody rigid = GetComponent<Rigidbody>();
        targetPostion =
            mainCameraObj.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0, 0);

        transform
            .GetComponent<Renderer>()
            .material.DOColor(
                new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)),
                0
            );

        Vector3 v = CalculateInitialVelocity(transform.position, targetPostion, 80f);
        rigid.velocity = v;
        rigid.angularVelocity = new Vector3(
            Random.Range(0f, 1f) * 180,
            Random.Range(0f, 1f) * 180,
            Random.Range(0f, 1f) * 180
        );
    }

    public static Vector3 CalculateInitialVelocity(
        Vector3 startPoint,
        Vector3 endPoint,
        float angle,
        float gravity = 9.81f
    )
    {
        Vector3 dir = endPoint - startPoint;
        float height = dir.y;
        dir.y = 0;
        float dist = dir.magnitude;
        float a = angle * Mathf.Deg2Rad;
        dir.y = dist * Mathf.Tan(a);
        dist += height / Mathf.Tan(a);

        float vel = Mathf.Sqrt(
            Mathf.Pow(dist, 2)
                * gravity
                / (2 * (dist * Mathf.Tan(a) - height) * Mathf.Pow(Mathf.Cos(a), 2))
        );
        return vel * dir.normalized;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (
            other.gameObject.CompareTag("Enemy")
            || other.gameObject.CompareTag("Player")
            || other.gameObject.CompareTag("MainCamera")
        )
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();

            if (targetHealth != null)
            {
                targetHealth.TakeDamage(10);
            }

            Destroy(gameObject);
        }
    }
}
