using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Singleton<EnemyController>
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;


    public event Action<float> OnTakeDamageEvent = null;
    public event Action OnDieEvent = null;

    private void Start() {
        TurnSystem.Instance.OnTurnChanged += HandleTurnChanged;
    }

    private void HandleTurnChanged(int turnCount, bool isPlayerTurn) {
        if (isPlayerTurn) return;

        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn() {
        yield return new WaitForSeconds(3.0f);

        TurnSystem.Instance.NextTurn();
    }

    public void Damage(int damage) {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            Debug.Log("DIE");
            OnDieEvent?.Invoke();
            return;
        }
        OnTakeDamageEvent?.Invoke(GetHealthPercentage());
    }

    public float GetHealthPercentage() {
        return (float)currentHealth / maxHealth;
    }
}
