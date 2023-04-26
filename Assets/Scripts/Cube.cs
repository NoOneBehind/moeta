using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cube : MonoBehaviour
{
    private GameObject mainCameraObj;
    // Start is called before the first frame update
    void Start()
    {

        mainCameraObj = GameObject.FindWithTag("MainCamera");
        Vector3 targetPosition = mainCameraObj.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0, 0);
        transform.DOJump(new Vector3(0,3,0), 2f, 1, 1f).SetEase(Ease.Linear);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
