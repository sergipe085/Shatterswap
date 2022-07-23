using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EnemyJuice : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController = null;
    [SerializeField] private Transform enemySprite = null;
    private Vector2 initialPosition = Vector2.zero;

    [Header("--- GET DAMAGE SCREEN SHAKE ---")]
    [SerializeField] private CameraShakePropertiesSO damageScreenShake = null;

    private void Awake() {
        if  (!enemyController) return;

        enemyController.OnTakeDamageEvent += EnemyController_OnTakeDamage;
        enemyController.OnDieEvent += EnemyController_OnDie;

        DOTween.Init();
    }

    private void Start() {
        initialPosition = enemySprite.transform.position;
    }

    private void Update() {
        enemySprite.transform.position = initialPosition + Vector2.up * Mathf.Sin(Time.time) * 0.2f;
    }

    private void EnemyController_OnTakeDamage(float healthPercentage) {
        CameraShake.Instance.Shake(damageScreenShake.duration, damageScreenShake.force, damageScreenShake.multiplyier);
        enemySprite.transform.DOPunchScale(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 0.4f, 0, 0);
    }

    private void EnemyController_OnDie() {
        SceneTransition.Instance.LoadScene(SceneManager.GetActiveScene().name);
        enemySprite.transform.DOPunchScale(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 0.4f, 0, 0);
        CameraShake.Instance.Shake(damageScreenShake.duration, damageScreenShake.force, damageScreenShake.multiplyier);
    }
}
