using System.Collections;
using UnityEngine;

public class GameOverAnimation : MonoBehaviour
{
    public Animator gameoverAnimator;
    public GameObject meshBox;

    void Start()
    {
        meshBox.SetActive(false);
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(3);
        gameoverAnimator.SetTrigger("PlayGameOverAnimation");

        yield return new WaitWhile(() => gameoverAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        // This waits until the current animation is done playing (normalizedTime becomes 1 when the animation is done).
        meshBox.SetActive(true);
    }
}
