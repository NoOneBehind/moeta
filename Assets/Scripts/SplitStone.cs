using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SplitStone : Stone
{
    [SerializeField]
    private GameObject babyStonePrefab;

    private GameObject rightController;
    private ActionBasedController controller;

    public void StartSplit()
    {
        rightController = GameObject.Find("RightHand Controller");
        controller = rightController.GetComponent<ActionBasedController>();
        
        StartCoroutine(StoneThrown());
    }

    private IEnumerator StoneThrown()
    {
        while (true)
        {
            if (controller.activateActionValue.action.ReadValue<float>() > 0.9f)
            {
                Split();
            }
                
            yield return null;
        }
    }

    private void Split()
    {
        Vector3 originalVel = rigid.velocity;

        GameObject[] babyStones = new GameObject[5];
        for (int i = 0; i < babyStones.Length; i++)
        {
            babyStones[i] = Instantiate(
                babyStonePrefab,
                transform.position,
                transform.rotation
            );

            babyStones[i].GetComponent<Rigidbody>().velocity = originalVel
                + new Vector3(0f, Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }

        Destroy(gameObject);
    }
}
