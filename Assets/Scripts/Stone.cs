using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stone : MonoBehaviour
{
    private Vector3 targetPostion;
    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 targetPostion, float angle, float gravity = 9.81f)
    {
        Vector3 v = CalculateInitialVelocity(transform.position, targetPostion, angle, gravity);
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
        float theta = angle * Mathf.Deg2Rad;
        dir.y = dist * Mathf.Tan(theta);

        float vel = Mathf.Sqrt(
            Mathf.Pow(dist, 2)
                * gravity
                / (2 * (dist * Mathf.Tan(theta) - height) * Mathf.Pow(Mathf.Cos(theta), 2))
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
