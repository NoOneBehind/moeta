using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class StoneSelector : MonoBehaviour
{
    private GameObject rightController;
    private GameObject rayIneteractController;
    private ActionBasedController controller;
    private XRRayInteractor rayInteractor;
    private XRInteractorLineVisual lineVisual;

    private GameObject canvas;
    private Canvas selectUICanvas;

    void Start()
    {
        rightController = GameObject.Find("RightHand Controller");
        rayIneteractController = GameObject.Find("LeftRayInteractController");
        controller = rightController.GetComponent<ActionBasedController>();

        rayInteractor = rayIneteractController.GetComponent<XRRayInteractor>();
        lineVisual = rayIneteractController.GetComponent<XRInteractorLineVisual>();

        canvas = gameObject.transform.GetChild(0).gameObject;
        selectUICanvas = canvas.GetComponent<Canvas>();

        StartCoroutine(SelectUI());
    }

    private IEnumerator SelectUI()
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
            }
            yield return null;
        }
    }
}
