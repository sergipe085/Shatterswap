using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJuice : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController = null;

    [Header("--- GET DAMAGE SCREEN SHAKE ---")]
    [SerializeField] private CameraShakePropertiesSO damageScreenShake = null;

    private void Awake() {
        if  (!enemyController) return;

        enemyController.OnTakeDamageEvent += EnemyController_OnTakeDamage;
    }

    private void EnemyController_OnTakeDamage(float healthPercentage) {
        CameraShake.Instance.Shake(damageScreenShake.duration, damageScreenShake.force, damageScreenShake.multiplyier);
    }
}
