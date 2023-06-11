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

    public bool isDead = false;

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

        for (int i = 0; i < 3; ++i)
        {
            if (animator != null)
                animator?.SetBool("isMoving", true);
            yield return StartCoroutine(moving.MoveToPoint(movePointPos[i]));
            if (isDead)
                yield break;
            yield return StartCoroutine(
                moving.RotateTowards(player.transform.position, rotateSpeed)
            );
            if (isDead)
                yield break;
            if (animator != null)
                animator?.SetBool("isMoving", false);
            yield return new WaitForSeconds(minimumMoveInterval + Random.Range(0f, 2f));
            if (isDead)
                yield break;
        }

        // attack
        while (!isDead)
        {
            attacking.Attack(stonePrefab, Random.Range(30f, 60f));
            yield return new WaitForSeconds(minimumAttackInterval + Random.Range(0f, 3f));
            if (isDead)
                yield break;
        }
    }

    protected void OnEnemyDeath()
    {
        gameManager.leftEnemyCount -= 1;
        healthBar.SetHealth(0);
        if (animator != null)
            animator?.SetTrigger("isDead");
        isDead = true;

        GetComponent<BoxCollider>().enabled = false;
        if (agent != null)
            agent.enabled = false;
        var moving = GetComponent<Moving>();
        if (moving != null)
            moving.enabled = false;

        var isFinal = gameManager.currentLevel == gameManager.maxLevel;

        if (gameManager.leftEnemyCount == 0 && !isFinal)
        {
            gameManager.currentLevel += 1;
            gameManager.LevelStart();
        }
        else if (isFinal)
        {
            gameManager.GameClear();
        }

        StopAllCoroutines();
        if (moving != null)
            moving.StopAllCoroutines();

        var destroyDelayTime = isFinal ? 0 : 3;

        Destroy(healthBar.gameObject, destroyDelayTime);
        Destroy(gameObject, destroyDelayTime);
    }

    protected void OnEnemyHealthChanged(int currentHealth)
    {
        healthBar.SetHealth(currentHealth);
        if (currentHealth > 0)
        {
            if (animator != null)
                animator?.SetTrigger("isHit");
        }
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
