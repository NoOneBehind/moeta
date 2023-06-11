using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Player : Health
{
    [SerializeField]
    private Stone stonePrefab;

    [SerializeField]
    public float throwAngle = 10f;

    [SerializeField]
    private TextMeshProUGUI healthText;

    protected override void Start()
    {
        base.Start();

        OnDeath += OnPlayerDeath;
        OnHealthChanged += OnPlayerHealthChanged;

        healthText.text = "Player HP : " + currentHealth.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 가장 가까운 적을 찾는다
            GameObject closestEnemy = FindClosestEnemy();

            if (closestEnemy != null)
            {
                Stone stone = Instantiate(
                    stonePrefab,
                    transform.position + Vector3.back * 2,
                    Quaternion.identity
                );
                stone.GetComponent<Renderer>().material.DOColor(Color.green, 0);

                stone.Throw(closestEnemy.transform.position - Vector3.forward * 1, 60);
            }
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = enemy;
                distance = curDistance;
            }
        }

        return closest;
    }

    void OnPlayerDeath()
    {
        GameManager.Instance.GameOver();
    }

    void OnPlayerHealthChanged(int currentHealth)
    {
        healthText.text = "Player HP : " + currentHealth.ToString();
    }
}
