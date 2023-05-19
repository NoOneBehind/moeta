using System.Collections;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    private GameObject player;
    private GameObject stonePrefab;

    public IEnumerator Attack(GameObject stonePrefab, float throwAngle)
    {
        player = GameObject.Find("Main Camera");
        GameObject stone = Instantiate(
            stonePrefab,
            transform.position,
            transform.rotation
        );
        Stone _stone = stone.GetComponent<Stone>();
        _stone.Throw(player.transform.position, throwAngle);

        yield break;
    }
}
