using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cubePrefab;

    [SerializeField]
    private Transform cubeSpawnTransform;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnCube), 3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCube()
    {
        Instantiate(cubePrefab, cubeSpawnTransform);
    }
}
