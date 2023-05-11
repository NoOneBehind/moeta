using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : Health
{
    protected override void Start()
    {
        base.Start();

        OnDeath += OnPlayerDeath;
        OnDeath += GameManager.Instance.GameOver;
        OnHealthChanged += OnPlayerHealthChanged;
    }

    void OnPlayerDeath()
    {
        Debug.Log("Player is dead");
    }

    void OnPlayerHealthChanged(int currentHealth)
    {
        Debug.Log("Player's health is changed to " + currentHealth);
    }
}
