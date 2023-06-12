using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class StoneSelector : MonoBehaviour
{
    [SerializeField]
    public int currentStoneMode;
    [SerializeField]
    private int maxSpecialStone = 10;
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
    private IXRSelectInteractor directInteractor;
    private XRRayInteractor rayInteractor;
    private XRInteractorLineVisual lineVisual;

    private GameObject canvas;
    private GameObject mommyButton;
    private GameObject mommyButtonImage;
    private Canvas selectUICanvas;

    void Start()
    {
        stoneList.Add(normalStone);
        stoneList.Add(boostStone);
        stoneList.Add(mommyStone);

        rightController = GameObject.Find("RightHand Controller");
        rayIneteractController = GameObject.Find("LeftRayInteractController");
        controller = rightController.GetComponent<ActionBasedController>();

        interactionManager
            = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();

        directInteractor = rightController.GetComponent<IXRSelectInteractor>();
        rayInteractor = rayIneteractController.GetComponent<XRRayInteractor>();
        lineVisual = rayIneteractController.GetComponent<XRInteractorLineVisual>();

        canvas = gameObject.transform.GetChild(0).gameObject;
        selectUICanvas = canvas.GetComponent<Canvas>();
        selectUICanvas.worldCamera = Camera.main;

        mommyButton = canvas.transform.GetChild(2).gameObject;
        mommyButtonImage = mommyButton.transform.GetChild(0).gameObject;

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

            // When triggered && current level >= 2
            while(
                controller.activateAction.action.ReadValue<float>() > 0.9f
                && GameManager.Instance.currentLevel != 1
            )
            {
                // Turn on UI
                if (!selectUICanvas.enabled)
                    selectUICanvas.enabled = true;
                if (!rayInteractor.enabled && !lineVisual.enabled)
                {
                    rayInteractor.enabled = true;
                    lineVisual.enabled = true;
                }

                if (GameManager.Instance.currentLevel == 2)
                {
                    mommyButton.GetComponent<Button>().enabled = false;
                    mommyButtonImage.GetComponent<Image>().enabled = false;
                }
                else
                {
                    mommyButton.GetComponent<Button>().enabled = true;
                    mommyButtonImage.GetComponent<Image>().enabled = true;
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
            switchedStone.GetComponent<StoneSelector>().currentStoneMode = 0;
            interactionManager.SelectEnter(directInteractor, switchedStone.GetComponent<IXRSelectInteractable>());
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
            switchedStone.GetComponent<StoneSelector>().currentStoneMode = 1;
            interactionManager.SelectEnter(directInteractor, switchedStone.GetComponent<IXRSelectInteractable>());
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
            switchedStone.GetComponent<StoneSelector>().currentStoneMode = 2;
            interactionManager.SelectEnter(directInteractor, switchedStone.GetComponent<IXRSelectInteractable>());
            Destroy(gameObject);
        }
    }
}
