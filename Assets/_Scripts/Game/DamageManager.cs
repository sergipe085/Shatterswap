using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : Singleton<DamageManager>
{
    private void Start() {
        TurnSystem.Instance.OnTurnChanged += HandleTurnChanged;
    }

    private void HandleTurnChanged(int turnCount, bool isPlayerTurn) {
        
    }

    private void DoDamageToEnemy(int damage) {
        EnemyController.Instance.Damage(damage);
    }

    public void DoDamage(Match[] matches) {
        MemberRepositorySO memberRepositorySO = Player.Instance.GetMemberRepositorySO();

        int damage = 0;
        foreach(Match match in matches) {
            if (match.GetLength() <= 2) continue;

            MemberSO memberWithAffinity = memberRepositorySO.GetMemberWithAffinityTo(match.item);

            if (!memberWithAffinity) continue;

            damage += memberWithAffinity.baseDamage * match.GetLength();
        }

        if (damage <= 0) return;

        DoDamageToEnemy(damage);
    }
}
