using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJuice : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private CameraShakePropertiesSO succededSwapShakeProperties = null;
    [SerializeField] private CameraShakePropertiesSO failedSwapShakeProperties = null;

    private void Awake() {
        if (!player) return;

        player.OnTrySwapEvent += Player_OnTrySwap;
    }

    private void Player_OnTrySwap(bool swapped) {
        CameraShakePropertiesSO swapShakeProperties = swapped ? succededSwapShakeProperties : failedSwapShakeProperties;

        CameraShake.Instance.Shake(swapShakeProperties.duration, swapShakeProperties.force, swapShakeProperties.multiplyier);
    }
}
