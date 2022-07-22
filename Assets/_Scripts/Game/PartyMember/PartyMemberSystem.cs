using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberSystem : Singleton<PartyMemberSystem>
{
    [SerializeField] MemberRepositorySO memberRepositorySO = null;

    public List<MemberSO> GetMemberList() {
        return memberRepositorySO.memberSOList;
    }

    public MemberRepositorySO GetMemberRepositorySO() {
        return memberRepositorySO;
    }
}
