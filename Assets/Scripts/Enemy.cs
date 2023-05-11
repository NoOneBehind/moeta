using UnityEngine;
using System.Collections;

public class Enemy : Health
{
    [SerializeField]
    private GameObject stonePrefab;

    [SerializeField]
    private float moveSpeed = 5;

    [SerializeField]
    float distance = 25f;

    [SerializeField]
    float attackInterval = 1f;

    private GameObject playerSpawnArea;
    private GameManager gameManager;

    protected override void Start()
    {
        base.Start();

        gameManager = GameManager.Instance;

        OnDeath += OnEnemyDeath;
        OnHealthChanged += OnEnemyHealthChanged;

        playerSpawnArea = GameObject.Find("PlayerSpawnArea");

        StartCoroutine(StartAction());
    }

    void OnEnemyDeath()
    {
        gameManager.leftEnemyCount -= 1;
        Destroy(gameObject);
    }

    void OnEnemyHealthChanged(int currentHealth)
    {
        Debug.Log("Enemy's health is changed to " + currentHealth);
    }

    private IEnumerator StartAction()
    {
        yield return StartCoroutine("MoveUntilReachAttackArea");
        yield return StartCoroutine("ThrowStone");
    }

    private IEnumerator MoveUntilReachAttackArea()
    {
        while (true)
        {
            transform.position += transform.forward * moveSpeed / 500;

            if (Vector3.Distance(transform.position, playerSpawnArea.transform.position) < distance)
            {
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator ThrowStone()
    {
        while (true)
        {
            GameObject stone = Instantiate(
                stonePrefab,
                transform.position + Vector3.forward * 2,
                Quaternion.identity
            );
            yield return new WaitForSeconds(attackInterval);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy collided with another enemy");
            StopCoroutine("MoveUntilReachAttackArea");
        }
    }
}
