using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stone : MonoBehaviour
{
    private Vector3 targetPostion;
    private Image filer;
    void Start()
    {
        GameObject mainCameraObj = GameObject.FindWithTag("MainCamera");        
        targetPostion = mainCameraObj.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0, 0);

        filer = GameObject.Find("Filter").GetComponent<Image>();

        transform.GetComponent<Renderer>().material.DOColor(new Color(Random.Range(0,1f), Random.Range(0,1f), Random.Range(0,1f)), 0);
    }

    public void ThrowWithAngle(float targetAngle)
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        Vector3 initialVel = CalculateInitialVelocity(transform.position, targetPostion, targetAngle);
        rigid.velocity= initialVel;
        rigid.angularVelocity = new Vector3(Random.Range(0f, 1f) * 180, Random.Range(0f, 1f) * 180, Random.Range(0f, 1f) * 180);
    }

    public static Vector3 CalculateInitialVelocity(Vector3 startPoint, Vector3 endPoint, float angle, float gravity = 9.81f) {
        Vector3 dir = endPoint - startPoint;
        float height = dir.y;
        dir.y = 0;
        float dist = dir.magnitude;
        float a = angle * Mathf.Deg2Rad;
        dir.y = dist * Mathf.Tan(a);
        dist += height / Mathf.Tan(a);

        float vel = Mathf.Sqrt( Mathf.Pow(dist, 2) * gravity / ( 2*(dist*Mathf.Tan(a) - height) * Mathf.Pow(Mathf.Cos(a), 2) ) );
        return vel * dir.normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            filer.DOColor(Color.red, 0);
            filer.DOFade(0, 1f);
        }
    }
}

