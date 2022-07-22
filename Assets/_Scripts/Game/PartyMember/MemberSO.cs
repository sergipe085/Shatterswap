using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MemberSO")]
public class MemberSO : ScriptableObject
{
    public string memberName = "joseph";
    public SwapType swapType = SwapType.BOTH;
    public Item affinity = null;
    public int baseDamage = 5;
}

public enum SwapType { VERTICAL , HORIZONTAL, BOTH, DIAGONAL }
