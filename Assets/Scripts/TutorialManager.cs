using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
public class TutorialManager : MonoBehaviour
{
    public AudioClip alienShipTakeoffSound;
    private AudioSource audioSource;

    public GameObject popUpWindow;
    public GameObject stonePrefab;
    public Transform target;
    public Transform spawnPoint;
    private int stonesThrown;
    private bool canGrabStone;
    private bool tutorialComplete;
    private int stonesHit; 
    public Image screenCover;
    private TextMeshProUGUI popUpText;

    private void Start()
    {
        popUpText = popUpWindow.GetComponentInChildren<TextMeshProUGUI>();
       //attackingScript = GameObject.Find("rockSpawn").GetComponent<Attacking>();

        Invoke("ShowFirstPopUp", 1f);
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void ShowFirstPopUp()
    {
        popUpWindow.SetActive(true);
  
      popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("자, 내가 돌을 다섯 번\n던질 테니 피해봐라!");

        Invoke("ThrowStone", 5f);
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
        popUpWindow.GetComponent<PopUpWindow>().ShowPopUp("좋아. 이번에야말로 옆 마을을 확실히 이길 수 있겠구나.\n하늘에 계신 어머니도 자랑스러워하실 거다.");
        Invoke("EndTutorial", 3f);
    }

    private void EndTutorial()
    {
        audioSource.PlayOneShot(alienShipTakeoffSound);
        StartCoroutine(BlinkAndFadeOut());

    }


    private IEnumerator BlinkAndFadeOut()
{
    for(int i = 0; i < 3; i++) // Blink 3 times
    {
        screenCover.DOColor(Color.white, 0.1f);
        yield return new WaitForSeconds(0.1f);
        screenCover.DOColor(Color.clear, 0.1f);
        yield return new WaitForSeconds(0.1f);
    }

    screenCover.DOColor(Color.black, 3f);
    yield return new WaitForSeconds(3f);



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
            Invoke("ShowSecondPopUp", 2f);
        }
    }

    public void StoneHitTarget()
    {
        if (tutorialComplete)
            return;

        stonesHit++;

        if (stonesHit == 5)
        {
            ShowThiredPopUp();
        }
    }

}
