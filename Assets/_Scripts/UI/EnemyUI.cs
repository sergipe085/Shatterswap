using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider = null;

    private void Start() {
        DOTween.Init();

        EnemyController.Instance.OnTakeDamageEvent += EnemyController_OnTakeDamage;
        EnemyController.Instance.OnDieEvent += () => UpdateHealthSlide(0.0f);
    }

    private void EnemyController_OnTakeDamage(float healthPercentage) {
        UpdateHealthSlide(healthPercentage);
    }

    private void UpdateHealthSlide(float healthPercentage) {
        DOTween.To(() => healthSlider.value, (x) => healthSlider.value = x, healthPercentage, 0.3f).SetEase(Ease.OutQuad);
        healthSlider.transform.DOPunchScale(new Vector3(0.5f, -0.5f), 0.2f);
    }
}
