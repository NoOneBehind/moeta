using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class StoneSelector : MonoBehaviour
{
    [SerializeField]
    private int currentStoneMode;
    [SerializeField]
    private GameObject normalStone;
    [SerializeField]
    private GameObject boostStone;
    [SerializeField]
    private GameObject mommyStone;
    private List<GameObject> stoneList = new List<GameObject>();

    private XRInteractionManager interactionManager;
    private GameObject rightController;
    private GameObject rayIneteractController;
    private ActionBasedController controller;
    private XRDirectInteractor directInteractor;
    private XRRayInteractor rayInteractor;
    private XRInteractorLineVisual lineVisual;

    private GameObject canvas;
    private Canvas selectUICanvas;

    void Start()
    {
        stoneList.Add(normalStone);
        stoneList.Add(boostStone);
        stoneList.Add(mommyStone);

        rightController = GameObject.Find("RightHand Controller");
        rayIneteractController = GameObject.Find("LeftRayInteractController");
        controller = rightController.GetComponent<ActionBasedController>();

        directInteractor = rightController.GetComponent<XRDirectInteractor>();
        rayInteractor = rayIneteractController.GetComponent<XRRayInteractor>();
        lineVisual = rayIneteractController.GetComponent<XRInteractorLineVisual>();

        canvas = gameObject.transform.GetChild(0).gameObject;
        selectUICanvas = canvas.GetComponent<Canvas>();
        selectUICanvas.worldCamera = Camera.main;

        StartCoroutine(SelectUIopen());
    }

    private IEnumerator SelectUIopen()
    {
        while (true)
        {
            if (selectUICanvas.enabled)
                selectUICanvas.enabled = false;
            if (rayInteractor.enabled && lineVisual.enabled)
            {
                rayInteractor.enabled = false;
                lineVisual.enabled = false;
            }

            while(controller.activateAction.action.ReadValue<float>() > 0.9f)
            {
                if (!selectUICanvas.enabled)
                    selectUICanvas.enabled = true;
                if (!rayInteractor.enabled && !lineVisual.enabled)
                {
                    rayInteractor.enabled = true;
                    lineVisual.enabled = true;
                }
                yield return null;
            }
            yield return null;
        }
    }

    public void SelectNormal()
    {
        if (currentStoneMode != 0)
        {
            GetComponent<SphereCollider>().enabled = false;
            GameObject switchedStone = Instantiate(
                stoneList[0],
                transform.position,
                transform.rotation
            );
            interactionManager.SelectEnter(directInteractor, switchedStone.GetComponent<XRGrabInteractable>());
            Destroy(gameObject);
        }
    }

    public void SelectBoost()
    {
        if (currentStoneMode != 1)
        {
            GetComponent<SphereCollider>().enabled = false;
            GameObject switchedStone = Instantiate(
                stoneList[1],
                transform.position,
                transform.rotation
            );
            interactionManager.SelectEnter(directInteractor, switchedStone.GetComponent<XRGrabInteractable>());
            Destroy(gameObject);
        }
    }

    public void SelectMommy()
    {
        if (currentStoneMode != 2)
        {
            GetComponent<SphereCollider>().enabled = false;
            GameObject switchedStone = Instantiate(
                stoneList[2],
                transform.position,
                transform.rotation
            );
            interactionManager.SelectEnter(directInteractor, switchedStone.GetComponent<XRGrabInteractable>());
            Destroy(gameObject);
        }
    }
}
