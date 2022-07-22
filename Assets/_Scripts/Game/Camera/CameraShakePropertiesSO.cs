using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Shake Properties")]
public class CameraShakePropertiesSO : ScriptableObject 
{
    public float duration;
    public float force;
    public float multiplyier;
}
