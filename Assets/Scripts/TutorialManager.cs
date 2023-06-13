using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit;


public class TutorialManager : MonoBehaviour
{
    public XRSocketInteractor socketInteractor;
    public AudioClip alienShipTakeoffSound;
    public AudioClip forestafternoon;
    private AudioSource audioSource;

    public GameObject popUpWindow;
    public GameObject stonePrefab;
    public Transform target;
    public Transform spawnPoint;
    private GameObject shield;
    private int stonesThrown;
    private bool canGrabStone;
    private bool tutorialComplete;
    private int stonesHit; 
    
    private TextMeshProUGUI popUpText;
    private bool isShieldTutorialDone = false;
    public bool onTutorial = true;
    private bool onThrowingTutorial = false;

    private void Start()
    {
        popUpText = popUpWindow.GetComponentInChildren<TextMeshProUGUI>();
       //attackingScript = GameObject.Find("rockSpawn").GetComponent<Attacking>();

        
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = forestafternoon;
        audioSource.loop = true;
        audioSource.Play();
        Invoke("ShowFirstPopUp", 1f);

        shield = GameObject.Find("Shield");

        onTutorial = true;
    }

    private void ShowFirstPopUp()
    {
        popUpWindow.SetActive(true);
  
      popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("자, 내가 돌을 다섯 번\n던질 테니 피해봐라!");

        Invoke("ThrowStone", 8f);
    }

    private IEnumerator ShowShieldTutorial()
    {
        yield return new WaitForSeconds(2.0f);

        popUpWindow.SetActive(true);
        popUpWindow.GetComponent<PopUpWindow>().ShowPopUp(
            "이번엔 방패 사용법을 익혀보자 \n 오른손으로 방패의 손잡이를 잡고 \n 왼쪽 손등에 고정시켜봐라!",
            8f
        );
        yield return StartCoroutine(CheckShieldAttached());
        yield return new WaitForSeconds(1f);

        popUpWindow.GetComponent<PopUpWindow>().ShowPopUp(
            "이번에도 돌을 다섯 번 던질건데, \n 방패로 막아봐라!"
        );
        stonesThrown = 0;
        isShieldTutorialDone = true;
        Invoke("ThrowStone", 8f);

        yield break;
    }

    private void ShowSecondPopUp()
    {
        popUpWindow.SetActive(true);
        popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("잘했다.\n 이제 상자의 돌을 집어 바위에 던져!\n  세 개를 맞춰보거라!");

        onThrowingTutorial = true;
        canGrabStone = true;
        stonesHit = 0;
    }

    private void ShowThiredPopUp()
    {
        popUpWindow.SetActive(true);
        popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("좋아. 이번에야말로 \n옆 마을을 확실히 이길 수 있겠구나.");
         StartCoroutine(ShowSecondPopupAfterDelay(3f));
}

        private IEnumerator ShowSecondPopupAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("하늘에 계신 어머니도\n 자랑스러워하실 거다.");
            Invoke("EndTutorial", 5f);
        }
    private void EndTutorial()
    {
        onTutorial = false;
        audioSource.Stop();
        audioSource.PlayOneShot(alienShipTakeoffSound);
       StartCoroutine(BlinkAndFadeOut());
}

    

    private IEnumerator BlinkAndFadeOut()
{ Image filer = GameObject.Find("Filter").GetComponent<Image>();
    for(int i = 0; i < 3; i++) // Blink 3 times
    {   
        filer.DOColor(Color.red, 0);
        filer.DOFade(0, 1f);
    }

    filer.DOColor(Color.black, 3f);
    filer.DOFade(0, 1f);
    Destroy(shield, 3f);
    yield return new WaitForSeconds(15f); // Wait for the fade out to finish

    UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial Scene2");

    }



    private void ThrowStone()
    {
        if (tutorialComplete)
            return;

         if (stonesThrown < 5)
        {
            //  Instantiate(stonePrefab, spawnPoint.position, Quaternion.identity);
            spawnPoint.GetComponent<Attacking>().Attack(stonePrefab, 45f); // Example throwAngle value of 45 degrees
            stonesThrown++;
            Invoke("ThrowStone", 2f);
        }
        else if (!isShieldTutorialDone)
        {
            StartCoroutine(ShowShieldTutorial());
            //Invoke("ShowThiredPopUp",3f);
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

        if (onThrowingTutorial)
            stonesHit++;

        if (stonesHit == 3)
        {
            Invoke("ShowThiredPopUp",3f);
        }
    }

    private IEnumerator CheckShieldAttached()
    {
        while (true)
        {
            if (socketInteractor.hasSelection)
                yield break;
            else
                yield return null;
        }
    }

}
