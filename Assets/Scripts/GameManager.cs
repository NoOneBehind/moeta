using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject stonePrefab;

    [SerializeField]
    private Transform stoneSpawnTransform;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnStone), 3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnStone()
    {
        Instantiate(stonePrefab, stoneSpawnTransform);
    }
}
