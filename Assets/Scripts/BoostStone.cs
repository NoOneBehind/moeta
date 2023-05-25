using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class BoostStone : Stone
{
    [SerializeField]
    private GameObject reticlePrefab;
    [SerializeField]
    private float timeLimit = 2.0f;
    private GameObject rayIneteractController;
    private ActionBasedController controller;    
    private XRRayInteractor rayInteractor;
    private GameObject reticle;
    private float timer = 0f;
    private bool isSlowed = false;
    private bool isBoosted = false;
    private float fixedDeltaTime;
    private Image filer;
    private GameObject cam;

    public void StartBoost()
    {
        rayIneteractController = GameObject.Find("RayInteractController");
        controller = rayIneteractController.GetComponent<ActionBasedController>();
        rayInteractor = rayIneteractController.GetComponent<XRRayInteractor>();

        fixedDeltaTime = Time.fixedDeltaTime;

        filer = GameObject.Find("Filter2").GetComponent<Image>();
        cam = GameObject.Find("Main Camera");

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
                
                // Disable ray interactor and Destory reticle
                if (
                    rayIneteractController.GetComponent<XRRayInteractor>().isActiveAndEnabled == true
                    && rayIneteractController.GetComponent<XRInteractorLineVisual>().isActiveAndEnabled == true
                )
                {
                    rayIneteractController.GetComponent<XRRayInteractor>().enabled = false;
                    rayIneteractController.GetComponent<XRInteractorLineVisual>().enabled = false;
                }

                if (reticle.activeSelf)
                {
                    Destroy(reticle);
                }

                // When boosted, show trail and gravity off
                if (isBoosted)
                {
                    GetComponent<TrailRenderer>().enabled = true;
                    filer.DOColor(Color.clear, 0);
                }
                yield return new WaitForSeconds(1.0f);
                GetComponent<Rigidbody>().useGravity = true;
                yield return new WaitForSeconds(2.0f);
                GetComponent<TrailRenderer>().enabled = false;                

                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator SlowTime()
    {
        // Add filter, Slow time
        Image filer = GameObject.Find("Filter2").GetComponent<Image>();
        filer.DOColor(new Color(1f, 0.6f, 0f, 0.3f), 0);
        filer.DOColor(Color.clear, timeLimit).SetEase(Ease.InOutQuad);

        Time.timeScale = 0.25f;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;

        // Add Ray Interactor
        if (
            rayIneteractController.GetComponent<XRRayInteractor>().isActiveAndEnabled == false
            && rayIneteractController.GetComponent<XRInteractorLineVisual>().isActiveAndEnabled == false
        )
        {
            rayIneteractController.GetComponent<XRRayInteractor>().enabled = true;
            rayIneteractController.GetComponent<XRInteractorLineVisual>().enabled = true;
        }

        // Add Ray Reticle
        reticle = Instantiate(reticlePrefab, new Vector3(0, -100.0f, 0), Quaternion.identity);

        // Raycast and boost the stone towards raycashit
        while( controller.selectActionValue.action.ReadValue<float>() > 0.9f )
        {
            timer += Time.deltaTime / Time.timeScale;

            RaycastHit res;
            if (rayInteractor.TryGetCurrent3DRaycastHit(out res))
            {
                Vector3 rayPoint = res.point;
                Vector3 dir = rayPoint - transform.position;
                Debug.Log(dir);
                float distance = (rayPoint - cam.transform.position).magnitude;

                // Update reticle position and scale
                reticle.transform.position = rayPoint;
                reticle.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f) * distance;
            }
            else
            {
                reticle.transform.position = new Vector3(0, -100.0f, 0);
            }

            if (controller.activateActionValue.action.ReadValue<float>() > 0.9f)
            {
                Boost(res.point);
                yield break;
            }

            if (timer >= timeLimit) yield break;

            yield return null;
        }

        yield break;
    }

    private void Boost(Vector3 point)
    {
        Vector3 dir = point - transform.position;
        Debug.Log("Final direction : " + dir);
        GetComponent<Rigidbody>().velocity = 40 * dir.normalized;
        Debug.Log("Boosted : " + GetComponent<Rigidbody>().velocity);
        GetComponent<Rigidbody>().useGravity = false;
        isBoosted = true;
    }
}