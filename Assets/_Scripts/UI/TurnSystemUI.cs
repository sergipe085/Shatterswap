using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton = null;
    [SerializeField] private TMPro.TextMeshProUGUI turnText = null;
    [SerializeField] private TMPro.TextMeshProUGUI memberText = null;

    private void Start() {
        endTurnButton.onClick.AddListener(TurnSystem.Instance.NextTurn);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        Player.Instance.OnMemberTurnChange += Testing_OnMemberTurnChanged;

        UpdateCurrentMember(Player.Instance.GetCurrentMember());
        UpdateVisual();
    }

    private void TurnSystem_OnTurnChanged(int turnCount, bool isPlayerTurn) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        bool isPlayerTurn = TurnSystem.Instance.IsPlayerTurn();
        endTurnButton.gameObject.SetActive(isPlayerTurn);
        memberText.gameObject.SetActive(isPlayerTurn);
        turnText.text = (isPlayerTurn ? "Player" : "Enemy") + " Turn.";
    }

    private void Testing_OnMemberTurnChanged(MemberSO memberSO) {
        UpdateCurrentMember(memberSO);
    }

    private void UpdateCurrentMember(MemberSO memberSO) {
        memberText.text = memberSO.memberName;
    }
}
