using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private GridObjectVisual debugPrefab = null;
    [SerializeField] private GameObject gridObjectVisualContainer;
    [SerializeField] private Vector2 gridSize = Vector2.one;
    [SerializeField] private GridSystem gridSystem;

    public event Action OnEndMove = null;

    private void Start() {
        gridSystem = new GridSystem(gridSize);
        gridSystem.CreateGridObjectVisuals(debugPrefab, gridObjectVisualContainer.transform);
        gridSystem.OnEndMove += () => OnEndMove?.Invoke();

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    public bool TrySwap(GridPosition gp1, GridPosition gp2, MemberSO memberSO) {
        return gridSystem.TrySwap(gp1, gp2, debugPrefab, memberSO);
    }

    public bool TrySwap(GridObject go1, GridObject go2, MemberSO memberSO) {
        return gridSystem.TrySwap(go1.gridPosition, go2.gridPosition, debugPrefab, memberSO);
    }

    private void TurnSystem_OnTurnChanged(int turnCount, bool isPlayerTurn) {
        if (isPlayerTurn) return;

        gridSystem.ClearMatching();
    }

    public bool IsGridPositionValid(GridPosition gridPosition) => gridSystem.IsGridPositionValid(gridPosition);

    public GridPosition WorldToGridPosition(Vector2 worldPosition) => gridSystem.WorldToGridPosition(worldPosition);

    public GridObject GetGridObjectByGridPosition(GridPosition gridPosition) => gridSystem.GetGridObjectByGridPosition(gridPosition);
}
