using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Enemy_temp : Health
{
    protected GameObject player;

    [Header("Health Property")]
    [SerializeField]
    public Vector3 healthBarOffset = new Vector3(0, 2.5f, 0);

    [SerializeField]
    GameObject healthBarPrefab;

    private HealthBar healthBar;

    [Space(10f)]
    [Header("Move Property")]
    [SerializeField]
    public GameObject[] movePoints;
    protected Vector3[] movePointPos;

    [SerializeField]
    protected float minimumMoveInterval = 5f;

    [SerializeField]
    protected float rotateSpeed = 200f;

    [Space(10f)]
    [Header("Attack Property")]
    [SerializeField]
    private GameObject stonePrefab;

    [SerializeField]
    protected float minimumAttackInterval = 5.0f;

    protected UnityEngine.AI.NavMeshAgent agent;

    protected Moving moving;
    protected Attacking attacking;
    protected Animator animator;
    protected GameManager gameManager;

    protected override void Start()
    {
        base.Start();
        gameManager = GameManager.Instance;

        OnDeath += OnEnemyDeath;
        OnHealthChanged += OnEnemyHealthChanged;

        moving = GetComponent<Moving>();
        attacking = GetComponent<Attacking>();
        animator = GetComponent<Animator>();

        player = GameObject.Find("Main Camera");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public IEnumerator MoveAndAttackLevel1()
    {
        movePointPos = new Vector3[movePoints.Length];
        for (int i = 0; i < movePoints.Length; i++)
        {
            movePointPos[i] =
                movePoints[i].transform.position
                + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        }

        // move to point1
        animator.SetBool("isMoving", true);
        yield return StartCoroutine(moving.MoveToPoint(movePointPos[0]));
        yield return StartCoroutine(moving.RotateTowards(player.transform.position, rotateSpeed));
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(minimumMoveInterval + Random.Range(0f, 2f));

        // move to point2
        animator.SetBool("isMoving", true);
        yield return StartCoroutine(moving.MoveToPoint(movePointPos[1]));
        yield return StartCoroutine(moving.RotateTowards(player.transform.position, rotateSpeed));
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(minimumMoveInterval + Random.Range(0f, 2f));

        // move to point3 (attack position)
        animator.SetBool("isMoving", true);
        yield return StartCoroutine(moving.MoveToPoint(movePointPos[2]));
        yield return StartCoroutine(moving.RotateTowards(player.transform.position, rotateSpeed));
        animator.SetBool("isMoving", false);

        // attack
        while (true)
        {
            attacking.Attack(stonePrefab, Random.Range(30f, 60f));
            yield return new WaitForSeconds(minimumAttackInterval + Random.Range(0f, 3f));
        }
    }

    protected void OnEnemyDeath()
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

    protected void OnEnemyHealthChanged(int currentHealth)
    {
        healthBar.SetHealth(currentHealth);
    }

    public void SetHealthBar()
    {
        Canvas canvas = gameObject.GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
        GameObject healthbarInstance = Instantiate<GameObject>(healthBarPrefab, canvas.transform);

        healthbarInstance.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        healthBar = healthbarInstance.GetComponent<HealthBar>();

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
        healthBar.target = gameObject.transform;
        healthBar.offset = healthBarOffset;
    }

    private void Update()
    {
        if (healthBar?.gameObject != null)
        {
            healthBar.gameObject.transform.LookAt(Camera.main.transform);
        }
    }
}
