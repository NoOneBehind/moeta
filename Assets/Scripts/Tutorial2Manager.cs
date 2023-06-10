using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial2Manager : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject dialogueBox1;
    public GameObject dialogueBox2;
    public GameObject dialogueBox3;


    private void Start()
    {
        DisplayInitialDialogue();
        dialogueBox1.SetActive(false);
        dialogueBox2.SetActive(false);
        dialogueBox3.SetActive(false);
        
    }

    private void DisplayInitialDialogue()
    {
      dialogueBox.SetActive(true);
    }

}
