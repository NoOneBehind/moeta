using UnityEngine;
using System.Collections;

public class tutorialStone : MonoBehaviour
{
    public Material redMaterial; // Your red material
    private Material originalMaterial; // To store the original material
    public TutorialManager tutorialManager;

    private void Start()
    {
        originalMaterial = GetComponent<Renderer>().material; // Save the original material at the start
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is a stone
        if (collision.gameObject.CompareTag("stone"))
        {
            // Notify the tutorial manager about the hit
            tutorialManager.StoneHitTarget();

            // Change the material of this object to red
            GetComponent<Renderer>().material = redMaterial;

            // Start a coroutine to revert the material back to original after 1 second
            StartCoroutine(RevertMaterial());
        }
    }

    IEnumerator RevertMaterial()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        GetComponent<Renderer>().material = originalMaterial; // Revert the material back to original
    }
}
