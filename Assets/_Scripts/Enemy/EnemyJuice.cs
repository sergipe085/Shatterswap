using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EnemyJuice : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController = null;
    [SerializeField] private Transform enemySprite = null;

    [Header("--- GET DAMAGE SCREEN SHAKE ---")]
    [SerializeField] private CameraShakePropertiesSO damageScreenShake = null;

    private void Awake() {
        if  (!enemyController) return;

        enemyController.OnTakeDamageEvent += EnemyController_OnTakeDamage;
        enemyController.OnDieEvent += EnemyController_OnDie;

        DOTween.Init();
    }

    private void EnemyController_OnTakeDamage(float healthPercentage) {
        CameraShake.Instance.Shake(damageScreenShake.duration, damageScreenShake.force, damageScreenShake.multiplyier);
        enemySprite.transform.DOPunchScale(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 0.4f, 0, 0);
    }

    private void EnemyController_OnDie() {
        CameraShake.Instance.Shake(2.0f, 0.05f, 1.006f, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        enemySprite.transform.DOPunchScale(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 0.4f, 0, 0);
    }
}
