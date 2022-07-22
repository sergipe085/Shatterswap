using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider = null;
    [SerializeField] private Image enemyImage = null;

    private void Start() {
        DOTween.Init();

        EnemyController.Instance.OnTakeDamageEvent += EnemyController_OnTakeDamage;
    }

    private void EnemyController_OnTakeDamage(float healthPercentage) {
        DOTween.To(() => healthSlider.value, (x) => healthSlider.value = x, healthPercentage, 0.3f).SetEase(Ease.OutQuad);
        healthSlider.transform.DOPunchScale(new Vector3(0.5f, -0.5f), 0.2f);
        enemyImage.rectTransform.DOPunchScale(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 0.4f, 0, 0);
    }
}
