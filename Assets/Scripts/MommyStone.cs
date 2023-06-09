using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class MommyStone : Stone
{
    [SerializeField]
    private GameObject babyStonePrefab;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private GameObject reticlePrefab;
    [SerializeField]
    private int maxBabyNum = 20;
    [SerializeField]
    private int minBabyNum = 3;
    [SerializeField]
    private float maxLandingSize = 10.0f;
    [SerializeField]
    private float timeLimit = 2.0f;
    private GameObject rightController;
    private ActionBasedController controller;
    private GameObject reticle;
    private float timer = 0f;
    private bool isThrown = false;
    private bool isSlowed = false;
    private bool isCollidedAfterThrown = false;
    private float fixedDeltaTime;
    private Image filer;
    private GameObject player;

    public void StartSplit()
    {
        rightController = GameObject.Find("RightHand Controller");
        controller = rightController.GetComponent<ActionBasedController>();

        fixedDeltaTime = Time.fixedDeltaTime;

        filer = GameObject.Find("Filter2").GetComponent<Image>();
        player = GameObject.Find("Main Camera");

        isThrown = true;

        StartCoroutine(StoneThrown());
    }

    private IEnumerator StoneThrown()
    {
        while (true)
        {
            if (
                controller.selectActionValue.action.ReadValue<float>() > 0.9f
                && isSlowed == false
            )
            {
                yield return StartCoroutine(SlowTime());

                isSlowed = true;

                // Return to the original state
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;

                // Destory reticle
                if (reticle.activeSelf)
                {
                    Destroy(reticle);
                }

                yield break;
            }

            if (isCollidedAfterThrown) yield break;

            yield return null;
        }
    }

    private IEnumerator SlowTime()
    {
        // Add filter, Slow time
        Image filer = GameObject.Find("Filter2").GetComponent<Image>();
        filer.DOColor(new Color(0.5f, 0f, 0.6f, 0.3f), 0);
        filer.DOColor(Color.clear, timeLimit).SetEase(Ease.InOutQuad);

        Time.timeScale = 0.25f;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;

        // Add landing point reticle
        Vector3 landingPoint = CalculateLandingPoint(transform.position, rigid.velocity);
        reticle = Instantiate(reticlePrefab, landingPoint, Quaternion.identity);
        while (controller.selectActionValue.action.ReadValue<float>() > 0.9f)
        {
            timer += Time.deltaTime / Time.timeScale;

            // Haptic feedback
            controller.SendHapticImpulse(0.1f, Time.deltaTime / Time.timeScale);

            // Increase baby stones number and landing point scale with time
            int babyNum = (int)(timer * (maxBabyNum - minBabyNum) / timeLimit + minBabyNum);
            float landingScale = timer * (maxLandingSize - 1f) / timeLimit + 1f;
            reticle.transform.localScale = new Vector3(landingScale, 1f, landingScale);

            // Split when triggered
            if (controller.activateActionValue.action.ReadValue<float>() > 0.9f)
            {
                controller.SendHapticImpulse(1.0f, 0.3f);
                Split(rigid.velocity, babyNum, false, landingScale, landingPoint);
                yield break;
            }

            if (timer >= timeLimit || isCollidedAfterThrown) yield break;

            yield return null;
        }

        yield break;
    }

    private void Split(
        Vector3 originalVel,
        int babyNum,
        bool isFromEnemy,
        float landingScale = 1f,
        Vector3 landingPoint = default(Vector3)
    )
    {
        player = GameObject.Find("Main Camera");

        // Original angle
        float originalAngle = Mathf.Atan2(
            originalVel.y,
            Vector3.Scale(originalVel, new Vector3(1f, 0f, 1f)).magnitude
            );

        // Add explosion effect
        GameObject explosion = Instantiate(
            explosionPrefab,
            transform.position,
            transform.rotation
        );
        Destroy(explosion, 2.0f);

        // Instantiate baby stones
        GameObject[] babyStones = new GameObject[babyNum];
        for (int i = 0; i < babyStones.Length; i++)
        {
            babyStones[i] = Instantiate(
                babyStonePrefab,
                transform.position
                    + new Vector3(
                        Random.Range(-0.1f, 0.1f),
                        Random.Range(-0.1f, 0.1f),
                        Random.Range(-0.1f, 0.1f)
                    ),
                transform.rotation
            );

            // Enemy baby stones' target is the player
            if (isFromEnemy)
            {
                babyStones[i].GetComponent<Rigidbody>().velocity = CalculateInitialVelocity(
                    transform.position,
                    player.transform.position
                        + new Vector3(
                            Random.Range(-0.5f * landingScale, 0.5f * landingScale),
                            Random.Range(-0.5f * landingScale, 0.5f * landingScale),
                            Random.Range(-0.5f * landingScale, 0.5f * landingScale)
                        ),
                originalAngle
                );
            }

            // Player baby stones' target is the original projectiles' landing point
            else
            {
                babyStones[i].GetComponent<Rigidbody>().velocity = CalculateInitialVelocity(
                    transform.position,
                    landingPoint
                        + new Vector3(
                            Random.Range(-0.5f * landingScale, 0.5f * landingScale),
                            0f,
                            Random.Range(-0.5f * landingScale, 0.5f * landingScale)
                        ),
                    originalAngle
                );
            }
        }

        Destroy(gameObject);
    }

    public IEnumerator EnemyThrowAndSplit(Vector3 targetPostion, float angle, float gravity = 9.81f)
    {
        base.Throw(targetPostion, angle, gravity);

        // Wait for reaching the highest point
        yield return new WaitForSeconds(CalculateHighestPointTime(rigid.velocity) + Random.Range(-0.1f, 0.1f));

        // Gravity off, Slow down stone
        rigid.useGravity = false;
        Vector3 originalVel = rigid.velocity;
        rigid.velocity = Vector3.Scale(rigid.velocity, new Vector3(0.1f, 0.1f, 0.1f));

        // Show laser towards player shortly
        LineRenderer _linRender = GetComponent<LineRenderer>();
        _linRender.enabled = true;
        _linRender.positionCount = 2;
        player = GameObject.Find("Main Camera");
        Vector3 target = player.transform.position - new Vector3(0f, 0.1f, 0f);
        _linRender.SetPosition(0, target);
        while (timer < 1.0f)
        {
            _linRender.SetPosition(1, transform.position);
            timer += Time.deltaTime;
            yield return null;
        }
        _linRender.enabled = false;

        // Split
        int babyNum = Random.Range(minBabyNum, maxBabyNum);
        float landingScale = Random.Range(1.0f, 10.0f);
        Split(originalVel, babyNum, true);

        yield break;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isThrown) isCollidedAfterThrown = true;
    }
}
