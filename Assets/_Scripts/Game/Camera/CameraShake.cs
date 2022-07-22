using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    private float currentShakeDuration = 0.0f;
    private float currentShakeForce = 0.0f;
    private float currentForceMultiplyier = 0.0f;

    private Vector3 initialPosition = new Vector3(0f, 0f, -10f);

    private void Update() {
        if (currentShakeDuration > 0.0f) {
            currentShakeDuration -= Time.deltaTime;

            DoShake();
        }
        else {
            transform.position = initialPosition;
        }
    }

    private void DoShake() {
        Vector2 shakePos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -10f) * currentShakeForce;
        currentShakeForce *= currentForceMultiplyier;
        transform.position = initialPosition + (Vector3)shakePos;
    }

    public void Shake(float duration, float force, float forceMultiplyier, bool aditive = false) {
        initialPosition = transform.position;

        if (aditive) {
            currentShakeDuration += duration;
            currentShakeForce += force;
        }
        else {
            currentShakeDuration = duration;
            currentShakeForce = force;
        }

        currentForceMultiplyier = forceMultiplyier;
    }
}
