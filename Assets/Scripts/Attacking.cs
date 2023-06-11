using System.Collections;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    private GameObject player;
    private GameObject stonePrefab;

    private Transform enemyStones;

    private void Awake()
    {
        enemyStones = GameObject.Find("EnemyStones").transform;
    }

    public void Attack(GameObject stonePrefab, float throwAngle)
    {
        player = GameObject.Find("Main Camera");
        GameObject stone = Instantiate(
            stonePrefab,
            transform.position + transform.forward * 3,
            transform.rotation,
            enemyStones
        );
        Stone _stone = stone.GetComponent<Stone>();
        _stone.Throw(player.transform.position, throwAngle);
    }

    public void BoostAttack(GameObject boostStonePrefab, float throwAngle)
    {
        player = GameObject.Find("Main Camera");
        GameObject boostStone = Instantiate(
            boostStonePrefab,
            transform.position + transform.forward * 3,
            transform.rotation,
            enemyStones
        );
        BoostStone _boostStone = boostStone.GetComponent<BoostStone>();
        StartCoroutine(_boostStone.EnemyThrowAndBoost(player.transform.position, throwAngle));
    }

    public void MommyAttack(GameObject mommyStonePrefab, float throwAngle)
    {
        player = GameObject.Find("Main Camera");
        GameObject mommyStone = Instantiate(
            mommyStonePrefab,
            transform.position + transform.forward * 3,
            transform.rotation,
            enemyStones
        );
        MommyStone _mommyStone = mommyStone.GetComponent<MommyStone>();
        StartCoroutine(_mommyStone.EnemyThrowAndSplit(player.transform.position, throwAngle));
    }
}
