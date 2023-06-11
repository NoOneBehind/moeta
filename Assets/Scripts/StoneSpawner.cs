using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject stonePrefab;
    [SerializeField]
    private int maxStoneNum = 30;

    private List<GameObject> stoneList = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnStone());
    }

    private IEnumerator SpawnStone()
    {
        // Instantiate stones at start
        for (int i = 0; i < maxStoneNum; i++)
        {
            GameObject newStone = Instantiate(
                stonePrefab,
                transform.position + new Vector3(
                        Random.Range(-0.2f, 0.2f),
                        Random.Range(0.1f, 0.3f),
                        Random.Range(-0.2f, 0.2f)
                    ),
                Quaternion.identity
                );
            stoneList.Add(newStone);

            yield return new WaitForSeconds(0.05f);
        }

        while (true)
        {
            // Instantiate new stone when stone is destroyed
            for (int i = 0; i < maxStoneNum; i++)
            {
                if (stoneList[i] == null)
                {
                    stoneList.RemoveAt(i);
                    AddNewStone();
                }
            }

            yield return null;
        }
    }

    private void AddNewStone()
    {
        GameObject newStone = Instantiate(
                stonePrefab,
                transform.position + new Vector3(
                        Random.Range(-0.2f, 0.2f),
                        Random.Range(0.1f, 0.3f),
                        Random.Range(-0.2f, 0.2f)
                    ),
                Quaternion.identity
                );
        stoneList.Add(newStone);
    }
}
