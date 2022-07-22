using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Testing : Singleton<Testing>
{
    private GridPosition gridSelect01 = GridPosition.Null();
    private GridPosition gridSelect02 = GridPosition.Null();

    public int currentMemberTurn = 0;
    [SerializeField] private List<MemberSO> memberList = new();
    [SerializeField] private MemberSO currentMember = null;

    public event Action<MemberSO> OnMemberTurnChange = null;

    protected override void Awake() {
        base.Awake();

        currentMember = memberList[0];
    }

    void Start()
    {
        GridManager.Instance.OnEndMove += GridManager_OnEndMove;

        OnMemberTurnChange?.Invoke(currentMember);
    }

    void Update()
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = MouseWorld.Instance.GetPosition();
            GridPosition gridPosition = GridManager.Instance.WorldToGridPosition(mousePosition);

            if (!GridManager.Instance.IsGridPositionValid(gridPosition)) return;

            if (gridSelect01 == GridPosition.Null()) {
                gridSelect01 = gridPosition;
            }
            else {
                gridSelect02 = gridPosition;

                GridManager.Instance.TrySwap(gridSelect01, gridSelect02, currentMember);

                gridSelect01 = GridPosition.Null();
                gridSelect02 = GridPosition.Null();
            }
            
        }
    }

    private void GridManager_OnEndMove() {
        Debug.Log("On end move");

        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        currentMemberTurn += 1;
        if (currentMemberTurn >= memberList.Count) {
            currentMemberTurn = 0;
            TurnSystem.Instance.NextTurn();
        }
        currentMember = memberList[currentMemberTurn];
        OnMemberTurnChange?.Invoke(currentMember);
    }

    public MemberSO GetCurrentMember() {
        return currentMember;
    }
}
