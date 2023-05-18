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

    [SerializeField]
    GameObject healthBarPrefab;

    public Vector3 healthBarOffset = new Vector3(0, 2.5f, 0);

    private HealthBar healthBar;
    private GameObject playerSpawnArea;
    private GameManager gameManager;

    protected override void Start()
    {
        base.Start();

        gameManager = GameManager.Instance;

        OnDeath += OnEnemyDeath;
        OnHealthChanged += OnEnemyHealthChanged;

        playerSpawnArea = GameObject.Find("PlayerSpawnArea");

        SetHealthBar();

        StartCoroutine(StartAction());
    }

    void SetHealthBar()
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject healthbarInstance = Instantiate<GameObject>(healthBarPrefab, canvas.transform);

        healthbarInstance.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        healthBar = healthbarInstance.GetComponent<HealthBar>();

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
        healthBar.target = gameObject.transform;
        healthBar.offset = healthBarOffset;
    }

    void OnEnemyDeath()
    {
        gameManager.leftEnemyCount -= 1;
        healthBar.SetHealth(0);

        if (gameManager.leftEnemyCount == 0)
        {
            gameManager.currentLevel += 1;
            gameManager.LevelStart();
        }

        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

    void OnEnemyHealthChanged(int currentHealth)
    {
        healthBar.SetHealth(currentHealth);
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
            stone.GetComponent<Stone>().Throw(playerSpawnArea.transform.position, 75f);
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
