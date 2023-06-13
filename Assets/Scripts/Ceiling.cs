using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling : MonoBehaviour
{
    [SerializeField]
    private GameObject primaryCeiling;
    [SerializeField]
    private GameObject secondaryCeiling;
    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private int hitCount = 0;

    void Start()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        hitCount++;
        if (hitCount > 15 && GameManager.Instance.currentLevel == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject explosion = Instantiate(
                    explosionPrefab,
                    transform.position + new Vector3(
                        Random.Range(-1f, 1f),
                        Random.Range(-2f, 0f),
                        Random.Range(-1f, 1f)
                    ),
                    Quaternion.identity
                );
                Destroy(explosion, 2.0f);
            }

            Destroy(secondaryCeiling);
            Destroy(primaryCeiling);
        }
    }
}
