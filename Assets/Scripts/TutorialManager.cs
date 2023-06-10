using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{

    public GameObject popUpWindow;
    public GameObject stonePrefab;
    public Transform target;

    private int stonesThrown;
    private bool canGrabStone;
    private bool tutorialComplete;

    private TextMeshProUGUI popUpText;

    private void Start()
    {
        popUpText = popUpWindow.GetComponentInChildren<TextMeshProUGUI>();
        attackingScript = FindObjectOfType<Attacking>();
        Invoke("ShowFirstPopUp", 1f);
    }

    private void ShowFirstPopUp()
    {
        popUpWindow.SetActive(true);
  
      popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("자, 내가 돌을 던져줄 테니\n 다섯 개만 피해 봐!");

        Invoke("ThrowStone", 5f);
    }

    private void ShowSecondPopUp()
    {
        popUpWindow.SetActive(true);
        popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("잘했다. 이제 바위에 던져!\n  다섯 개를 맞춰보자!");

        canGrabStone = true;
    }

    private void ShowThiredPopUp()
    {
        popUpWindow.SetActive(true);
        popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("좋아. 이번에야말로 옆 마을을 확실히 이길 수 있겠구나.하늘에 계신 어머니도 자랑스러워하실 거다.");
    }

    private void ThrowStone()
    {
        if (tutorialComplete)
            return;

         if (stonesThrown < 5)
        {
            attackingScript.Attack(stonePrefab, 45f); // Example throwAngle value of 45 degrees
            stonesThrown++;
            Invoke("ThrowStone", 1.5f);
        }
        else
        {
            Invoke("ShowSecondPopUp", 2f);
        }
    }

    public void StoneHitTarget()
    {
        if (tutorialComplete)
            return;

        stonesThrown--;

        if (stonesThrown == 0)
        {
            tutorialComplete = true;
            canGrabStone = false;
            // Move to the next situation or scene
        }
    }
}
