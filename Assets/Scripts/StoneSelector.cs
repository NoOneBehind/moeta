using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class StoneSelector : MonoBehaviour
{
    [SerializeField]
    public int currentStoneMode;
    [SerializeField]
    public static int maxSpecialStone = 10;
    [SerializeField]
    public static int leftBoostStones = 10;
    [SerializeField]
    public static int leftMommyStones = 10;
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
    private Rigidbody rigid;

    private GameObject canvas;
    private GameObject boostButton;
    private GameObject boostButtonImage;
    private Text boostLeftText;
    private GameObject mommyButton;
    private GameObject mommyButtonImage;
    private Text mommyLeftText;
    private Canvas selectUICanvas;
    private float fixedDeltaTime;
    private Image filer;

    private IEnumerator selectUICoroutine;

    [HideInInspector]
    public bool onSelecting = false;

    public void StartSelectUI()
    {
        stoneList.Add(normalStone);
        stoneList.Add(boostStone);
        stoneList.Add(mommyStone);

        rightController = GameObject.Find("RightHand Controller");
        rayIneteractController = GameObject.Find("LeftRayInteractController");
        controller = rightController.GetComponent<ActionBasedController>();

        fixedDeltaTime = Time.fixedDeltaTime;

        interactionManager
            = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();

        directInteractor = rightController.GetComponent<IXRSelectInteractor>();
        rayInteractor = rayIneteractController.GetComponent<XRRayInteractor>();
        lineVisual = rayIneteractController.GetComponent<XRInteractorLineVisual>();

        filer = GameObject.Find("Filter2").GetComponent<Image>();

        canvas = gameObject.transform.GetChild(0).gameObject;
        selectUICanvas = canvas.GetComponent<Canvas>();
        selectUICanvas.worldCamera = Camera.main;

        boostButton = canvas.transform.GetChild(1).gameObject;
        boostButtonImage = boostButton.transform.GetChild(0).gameObject;
        boostLeftText
            = boostButtonImage.transform.GetChild(0).gameObject.GetComponent<Text>();

        mommyButton = canvas.transform.GetChild(2).gameObject;
        mommyButtonImage = mommyButton.transform.GetChild(0).gameObject;
        mommyLeftText
            = mommyButtonImage.transform.GetChild(0).gameObject.GetComponent<Text>();

        selectUICoroutine = SelectUIopen();
        StartCoroutine(selectUICoroutine);
    }

    public void StopSelectUI()
    {
        Debug.Log("Throwed");
        if (selectUICanvas.enabled)
            selectUICanvas.enabled = false;
        if (rayInteractor.enabled && lineVisual.enabled)
        {
            rayInteractor.enabled = false;
            lineVisual.enabled = false;
        }
        Invoke("StoneCount", 0.1f);
    }

    private void StoneCount()
    {
        rigid = GetComponent<Rigidbody>();
        Debug.Log(rigid.velocity.magnitude);
        if (currentStoneMode == 1 && rigid.velocity.magnitude > 1)
            leftBoostStones--;
        if (currentStoneMode == 2 && rigid.velocity.magnitude > 1)
            leftMommyStones--;

        StopAllCoroutines();
    }

    private IEnumerator SelectUIopen()
    {
        while (true)
        {
            // Return to the original state
            // filer.DOColor(new Color(1f, 1f, 1f, 0f), 0);
            // Time.timeScale = 1.0f;
            // Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
            
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
                && GameManager.Instance?.currentLevel != null
                && GameManager.Instance?.currentLevel != 1
            )
            {
                // Add filter, Slow time
                // filer.DOColor(new Color(1f, 1f, 1f, 0.2f), 0);
                // Time.timeScale = 0.25f;
                // Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;

                // Turn on UI
                if (!selectUICanvas.enabled)
                    selectUICanvas.enabled = true;
                if (!rayInteractor.enabled && !lineVisual.enabled)
                {
                    rayInteractor.enabled = true;
                    lineVisual.enabled = true;
                }
                boostLeftText.text
                    = "Boost Stone\n" + leftBoostStones.ToString()
                    + " / " + maxSpecialStone.ToString();
                if (leftBoostStones > 0)
                    boostLeftText.color = Color.black;
                else
                    boostLeftText.color = Color.red;

                // Show mommy stone UI only on level 3
                if (
                    GameManager.Instance?.currentLevel != null
                    && GameManager.Instance?.currentLevel == 2
                )
                {
                    mommyButton.GetComponent<Button>().enabled = false;
                    mommyButton.GetComponent<Image>().enabled = false;
                    mommyButtonImage.GetComponent<Image>().enabled = false;
                    mommyLeftText.enabled = false;
                }
                else
                {
                    mommyButton.GetComponent<Button>().enabled = true;
                    mommyButton.GetComponent<Image>().enabled = true;
                    mommyButtonImage.GetComponent<Image>().enabled = true;
                    mommyLeftText.text
                        = "Explosion Stone\n" + leftMommyStones.ToString()
                    + " / " + maxSpecialStone.ToString();
                    if (leftMommyStones > 0)
                        mommyLeftText.color = Color.black;
                    else
                        mommyLeftText.color = Color.red;
                }

                onSelecting = true;

                yield return null;
            }

            onSelecting = false;
            yield return null;
        }
    }

    public void SelectNormal()
    {
        StopCoroutine(selectUICoroutine);
        if (currentStoneMode != 0)
        {
            GetComponent<SphereCollider>().enabled = false;
            GameObject switchedStone = Instantiate(
                stoneList[0],
                transform.position,
                transform.rotation
            );
            switchedStone.GetComponent<StoneSelector>().currentStoneMode = 0;
            switchedStone.GetComponent<StoneSelector>().StartSelectUI();
            interactionManager.SelectEnter(directInteractor, switchedStone.GetComponent<IXRSelectInteractable>());
            Destroy(gameObject);
        }
    }

    public void SelectBoost()
    {
        StopCoroutine(selectUICoroutine);
        if (currentStoneMode != 1 && leftBoostStones > 0)
        {
            GetComponent<SphereCollider>().enabled = false;
            GameObject switchedStone = Instantiate(
                stoneList[1],
                transform.position,
                transform.rotation
            );
            switchedStone.GetComponent<StoneSelector>().currentStoneMode = 1;
            switchedStone.GetComponent<StoneSelector>().StartSelectUI();
            interactionManager.SelectEnter(directInteractor, switchedStone.GetComponent<IXRSelectInteractable>());
            Destroy(gameObject);
        }
    }

    public void SelectMommy()
    {
        StopCoroutine(selectUICoroutine);
        if (currentStoneMode != 2 && leftMommyStones > 0)
        {
            GetComponent<SphereCollider>().enabled = false;
            GameObject switchedStone = Instantiate(
                stoneList[2],
                transform.position,
                transform.rotation
            );
            switchedStone.GetComponent<StoneSelector>().currentStoneMode = 2;
            switchedStone.GetComponent<StoneSelector>().StartSelectUI();
            interactionManager.SelectEnter(directInteractor, switchedStone.GetComponent<IXRSelectInteractable>());
            Destroy(gameObject);
        }
    }
}
