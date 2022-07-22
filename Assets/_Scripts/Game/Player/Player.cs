using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//LATER SEPERATE THIS CLASS
public class Player : Singleton<Player>
{
    // --- GRID SWAP --- //
    private GridPosition gridSelect01 = GridPosition.Null();
    private GridPosition gridSelect02 = GridPosition.Null();
    private GridObject goSelected01 = null;
    private GridObject goSelected02 = null;
    
    // --- MEMBER MANAGEMENT --- //
    public int currentMemberTurn = 0;
    [SerializeField] private MemberRepositorySO memberRepositorySO = null;
    [SerializeField] private MemberSO currentMember = null;

    public event Action<MemberSO> OnMemberTurnChange = null;
    public event Action<bool> OnTrySwapEvent = null;

    protected override void Awake() {
        base.Awake();

        currentMember = memberRepositorySO.memberSOList[0];
    }

    void Start()
    {
        GridManager.Instance.OnEndMove += GridManager_OnEndMove;

        OnMemberTurnChange?.Invoke(currentMember);
    }

    void Update() {
        if (!TurnSystem.Instance.IsPlayerTurn()) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = MouseWorld.Instance.GetPosition();
            GridPosition mouseGridPosition = GridManager.Instance.WorldToGridPosition(mousePosition);

            GridObject go = GridManager.Instance.GetGridObjectByGridPosition(mouseGridPosition);

            if (go == null) return;

            if (goSelected01 == null) {
                goSelected01 = go;
                goSelected01.Select();
            }
            else {
                if (go == goSelected01) return;

                goSelected02 = go;

                bool swapped = GridManager.Instance.TrySwap(goSelected01, goSelected02, currentMember);
                OnTrySwapEvent?.Invoke(swapped);

                goSelected01 = null;
                goSelected02 = null;
            }

            // if (gridSelect01 == GridPosition.Null()) {
            //     GridObject go = GridManager.Instance.
            //     gridSelect01 = gridPosition;
            // }
            // else {
            //     gridSelect02 = gridPosition;

            //     bool swapped = GridManager.Instance.TrySwap(gridSelect01, gridSelect02, currentMember);
            //     OnTrySwapEvent?.Invoke(swapped);

            //     gridSelect01 = GridPosition.Null();
            //     gridSelect02 = GridPosition.Null();
            // }
            
        }
    }

     private void GridManager_OnEndMove() {
        Debug.Log("On end move");

        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        currentMemberTurn += 1;
        if (currentMemberTurn >= memberRepositorySO.memberSOList.Count) {
            currentMemberTurn = 0;
            TurnSystem.Instance.NextTurn();
        }
        currentMember = memberRepositorySO.memberSOList[currentMemberTurn];
        OnMemberTurnChange?.Invoke(currentMember);
    }

    public MemberSO GetCurrentMember() {
        return currentMember;
    }

    public MemberRepositorySO GetMemberRepositorySO() {
        return memberRepositorySO;
    }
}
