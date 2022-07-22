using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MemberRepositorySO")]
public class MemberRepositorySO : ScriptableObject
{
    public List<MemberSO> memberSOList = new();

    public MemberSO GetMemberWithAffinityTo(Item itemToCheckAffinity) {
        return memberSOList.Find((memberSO) => memberSO.affinity == itemToCheckAffinity);
    }
}