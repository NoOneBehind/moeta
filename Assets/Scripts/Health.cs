using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    public int maxHealth;

    public int currentHealth;

    // 이벤트 핸들러를 선언합니다.
    public delegate void HealthChangedEventHandler(int currentHealth);
    public delegate void DeathEventHandler();

    // 이벤트를 선언합니다.
    public event HealthChangedEventHandler OnHealthChanged;
    public event DeathEventHandler OnDeath;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
