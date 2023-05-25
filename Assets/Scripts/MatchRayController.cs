using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRayController : MonoBehaviour
{
    [SerializeField]
    private GameObject parentController;

    void Start()
    {
        StartCoroutine(MatchController());
    }

    private IEnumerator MatchController()
    {
        while(true)
        {
            transform.position = parentController.transform.position;
            transform.rotation = parentController.transform.rotation;
            yield return null;
        }
    }

}
