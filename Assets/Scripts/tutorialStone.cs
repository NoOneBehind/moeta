using UnityEngine;

public class tutorialStone : MonoBehaviour
{
    public TutorialManager tutorialManager;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is a stone
        if (collision.gameObject.CompareTag("stone"))
        {
            // Notify the tutorial manager about the hit
            tutorialManager.StoneHitTarget();
        }
    }
}
