using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    private float currentShakeDuration = 0.0f;
    private float currentShakeForce = 0.0f;
    private float currentForceMultiplyier = 0.0f;
    private bool isShaking = false;

    private Vector3 initialPosition = new Vector3(0f, 0f, -10f);
    private Action OnCompleteAction = null;

    private void Update() {
        if (currentShakeDuration > 0.0f) {
            currentShakeDuration -= Time.deltaTime;

            DoShake();
        }
        else if (isShaking) {
            transform.position = initialPosition;
            isShaking = false;
            OnCompleteAction?.Invoke();
            OnCompleteAction = null;
        }
    }

    private void DoShake() {
        Vector2 shakePos = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), -10f) * currentShakeForce;
        currentShakeForce *= currentForceMultiplyier;
        transform.position = initialPosition + (Vector3)shakePos;
    }

    public void Shake(float duration, float force, float forceMultiplyier) {
        initialPosition = transform.position;

        currentShakeDuration = duration;
        currentShakeForce = force;

        currentForceMultiplyier = forceMultiplyier;
        isShaking = true;
    }

    public void Shake(float duration, float force, float forceMultiplyier, Action OnComplete) {
        OnCompleteAction = OnComplete;
        Shake(duration, force, forceMultiplyier);
    }
}
