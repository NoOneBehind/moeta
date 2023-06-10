using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour
{
    public AudioClip alienShipTakeoffSound;
    public AudioClip forestafternoon;
    private AudioSource audioSource;

    public GameObject popUpWindow;
    public GameObject stonePrefab;
    public Transform target;
    public Transform spawnPoint;
    private int stonesThrown;
    private bool canGrabStone;
    private bool tutorialComplete;
    private int stonesHit; 
    
    private TextMeshProUGUI popUpText;

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
    }

    private void ShowFirstPopUp()
    {
        popUpWindow.SetActive(true);
  
      popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("자, 내가 돌을 다섯 번\n던질 테니 피해봐라!");

        Invoke("ThrowStone", 8f);
    }

    private void ShowSecondPopUp()
    {
        popUpWindow.SetActive(true);
        popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("잘했다. 이제 바위에 던져!\n  다섯 개를 맞춰보거라!");

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
            Invoke("ThrowStone", 1.5f);
        }
        else
        {
            //Invoke("ShowSecondPopUp", 2f);
            Invoke("ShowThiredPopUp",3f);

        }
    }

    public void StoneHitTarget()
    {
        if (tutorialComplete)
            return;

        stonesHit++;

        if (stonesHit == 5)
        {
            Invoke("ShowThiredPopUp",3f);
        }
    }

}
