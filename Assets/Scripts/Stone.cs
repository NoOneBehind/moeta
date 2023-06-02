using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stone : MonoBehaviour
{
    protected Vector3 targetPostion;
    protected Rigidbody rigid;
    protected bool isCollided = false;

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

    public static float CalculateHighestPointTime(Vector3 initialVel, float gravity = 9.81f)
    {
        float velY = initialVel.y;
        return Mathf.Sqrt(2 * velY / gravity);
    }

    private void OnCollisionEnter(Collision other)
    {
        isCollided = true;
        
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

            // just for collision test
            if (other.gameObject.tag == "MainCamera")
            {
                Image filer = GameObject.Find("Filter").GetComponent<Image>();
                filer.DOColor(Color.red, 0);
                filer.DOFade(0, 1f);
            }

            if (!rigid.useGravity)
            {
                rigid.useGravity = true;
            }

            Destroy(gameObject, 5f);
        }
    }
}
