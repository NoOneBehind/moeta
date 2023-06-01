using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject popUpWindow;
    public GameObject stonePrefab;
    public Transform spawnPoint;
    public Transform target;

    private int stonesThrown;
    private bool canGrabStone;
    private bool tutorialComplete;

    private TextMeshProUGUI popUpText;

    private void Start()
    {
        popUpText = popUpWindow.GetComponentInChildren<TextMeshProUGUI>();
        Invoke("ShowFirstPopUp", 1f);
    }

    private void ShowFirstPopUp()
    {
        popUpWindow.SetActive(true);
        popUpText.text = "first text";
        //popUpText.text = "자, 내가 돌을 던져줄 테니 다섯 개만 피해 봐!";

        Invoke("ThrowStone", 5f);
    }

    private void ShowSecondPopUp()
    {
        popUpWindow.SetActive(true);
        popUpText.text = "second text";
        //popUpText.text = "아버지: 잘했다. 이제 바위에 던져! 다섯 개를 맞춰보자!";

        canGrabStone = true;
    }

    private void ThrowStone()
    {
        if (tutorialComplete)
            return;

        if (stonesThrown < 5)
        {
            Instantiate(stonePrefab, spawnPoint.position, Quaternion.identity);
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
