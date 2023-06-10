using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial2Manager : MonoBehaviour
{
    public GameObject dialogueText;
    public GameObject choice1;
    public GameObject choice2;

    private void Start()
    {
        DisplayInitialDialogue();
    }

    private void DisplayInitialDialogue()
    {
        dialogueText.GetComponent<PopUpWindow>().ShowPopUp("석아! 정신이 드느냐? 너희 아버지가 너를 지키려다가 잡혀가셨어!");
        choice1.GetComponent<PopUpWindow>().ShowPopUp ("네? 그럼 어서 아버지를 구하러 가요!");
        choice2.GetComponent<PopUpWindow>().ShowPopUp ("무엇이 아버지를 잡아갔나요?");
    }

    public void OnChoice1Clicked()
    {
        dialogueText.GetComponent<PopUpWindow>().ShowPopUp("하지만 한양에서 군대가 오려면 아직 기다려야 한다.");
        //dialogueText.text = "하지만 한양에서 군대가 오려면 아직 기다려야 한다.";
        ContinueDialogue();
    }

    public void OnChoice2Clicked()
    {
        dialogueText.GetComponent<PopUpWindow>().ShowPopUp("하늘에서 날라온 이상한 접시가 너희 아버지를 잡아갔단다.");
        //dialogueText.text = "하늘에서 날라온 이상한 접시가 너희 아버지를 잡아갔단다.";
        ContinueDialogue();
    }

    private void ContinueDialogue()
    {
        choice1.GetComponent<PopUpWindow>().ShowPopUp("기다릴 시간이 없어요. ...돌! 돌이라도 던져보겠습니다!");
        choice2.SetActive(false); // Hide the second choice as there's only one option now
    }

    public void OnFinalChoiceClicked()
    {
        // Transition to main scene
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene"); // Replace with the actual name of your main scene
    }
}
