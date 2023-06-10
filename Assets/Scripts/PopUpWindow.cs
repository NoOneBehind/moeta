using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpWindow : MonoBehaviour
{
    public TextMeshProUGUI popUpText;
    public float displayTime = 5f;

    private void Start()
    {
        // Hide the pop-up window initially
        gameObject.SetActive(false);
    }

    public void ShowPopUp(string text)
    {
        // Set the text of the pop-up window
        popUpText.text = text;

        // Show the pop-up window
        gameObject.SetActive(true);

        // Start a coroutine to hide the pop-up window after a certain time
        StartCoroutine(HidePopUpAfterDelay());
    }

    

    private IEnumerator HidePopUpAfterDelay()
    {
        // Wait for the specified display time
        yield return new WaitForSeconds(displayTime);

        // Hide the pop-up window
        gameObject.SetActive(false);
    }
}
