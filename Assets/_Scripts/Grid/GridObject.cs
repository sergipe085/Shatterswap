using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridObject
{
    public Item item = null;
    private GridSystem gridSystem;
    public GridPosition gridPosition;
    public Match currentMatch = null;

    public event Action OnGridObjectChange = null;
    public event Action OnGridStopChange = null;
    public event Action OnDestroyed = null;
    public event Action OnSelectEvent = null;

    public GridObject(GridSystem _gridSystem, GridPosition _gridPosition, Item _item) {
        this.gridSystem = _gridSystem;
        this.gridPosition = _gridPosition;
        this.item = _item;
    }

    public GridPosition GetPosition() {
        return gridPosition;
    }

    public Vector2 GetWorldPosition() {
        return gridSystem.GridToWorldPosition(GetPosition());
    }

    public Item GetItem() {
        return item;
    }

    public void ChangeItem(Item newItem) {
        item = newItem;
        OnGridObjectChange?.Invoke();
    }

    public void UpdateVisual() {
        OnGridStopChange?.Invoke();
    }

    public void Delete() {
        ChangeItem(null);
        OnDestroyed?.Invoke();
    }

    public void Regenerate(Item _item) {
        ChangeItem(_item);
        OnGridStopChange?.Invoke();
    }

    private bool CheckGridObjectMatchWithMe(GridObject _gridObject) {
        if (_gridObject == null) return false;

        return item != null && _gridObject.GetItem() != null && item == _gridObject.GetItem();
    }

    public GridSystem GetGridSystem() {
        return gridSystem;
    }

    public void Select() {
        OnSelectEvent?.Invoke();
    }
}
