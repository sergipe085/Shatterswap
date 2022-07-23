using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class GridSystem
{
    [SerializeField] private List<Match> matches = new();
    private GridObject [,] gridArray;
    private Vector2 size = Vector2.zero;
    private float cellSize = 1f;

    private bool isBusy = false;

    public event Action OnEndMove = null;

    private Vector3 positionOffset = Vector3.zero;

    public GridSystem(Vector2 _size) {
        this.size = _size;

        gridArray = new GridObject[(int)size.x, (int)size.y];

        for(int x = 0; x < size.x; x++) {
            for(int y = 0; y < size.y; y++) {
                GridPosition gridPosition = new GridPosition(x, y);
                Item item = ItemContainer.Instance.GetRandomItem();
                GridObject newGridObject = new GridObject(this, gridPosition, item);
                gridArray[x, y] = newGridObject;
            }
        }

        CheckForMatches();
    }

    public void CreateDebugObjects(Transform debugPrefab) {
        for(int x = 0; x < size.x; x++) {
            for(int y = 0; y < size.y; y++) {
                GameObject.Instantiate(debugPrefab.gameObject, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    public void CreateGridObjectVisuals(GridObjectVisual _gridObjectVisual, Transform parent, Vector2 offsetPosition) {
        positionOffset = offsetPosition;

        for(int x = 0; x < size.x; x++) {
            for(int y = 0; y < size.y; y++) {
                GridObjectVisual gridObjectVisual = GameObject.Instantiate<GridObjectVisual>(_gridObjectVisual, positionOffset + new Vector3(x - size.x / 2, y - size.y / 2, 0), Quaternion.identity);
                gridObjectVisual.Initialize(gridArray[x, y]);
                gridObjectVisual.transform.SetParent(parent);
            }
        }
    }

    public GridPosition WorldToGridPosition(Vector2 worldPosition) {
        GridPosition gridPosition = new GridPosition(Mathf.RoundToInt(((worldPosition.x - positionOffset.x) / cellSize) + size.x / 2), Mathf.RoundToInt(((worldPosition.y - positionOffset.y) / cellSize) + size.y / 2));
        return gridPosition;
    }

    public Vector2 GridToWorldPosition(GridPosition gridPosition) {
        return new Vector2(gridPosition.x - size.x / 2, gridPosition.y - size.y / 2) + (Vector2)positionOffset;
    }

    public GridObject GetGridObjectByWorldPoint(Vector2 worldPosition) {
        GridObject gridObject = gridArray[Mathf.RoundToInt(((worldPosition.x - positionOffset.x) / cellSize) + size.x / 2), Mathf.RoundToInt(((worldPosition.y - positionOffset.y) / cellSize) + size.y / 2)];
        return gridObject;
    }

    public GridObject GetGridObjectByGridPosition(GridPosition gridPosition) {
        if (!IsGridPositionValid(gridPosition)) return null;

        return gridArray[gridPosition.x, gridPosition.y];
    }

    public bool IsGridPositionValid(GridPosition gridPosition) {
        if (gridPosition.x < 0 || gridPosition.y < 0) return false;
        if (gridPosition.x >= gridArray.GetLength(0) || gridPosition.y >= gridArray.GetLength(1)) return false;
        return true;
    }

    public bool TrySwap(GridPosition gp1, GridPosition gp2, GridObjectVisual itemImageBuffer, MemberSO memberSO) {
        if (isBusy) return false;

        if (gp1 == gp2) return false;

        if (!IsGridPositionValid(gp1) || !IsGridPositionValid(gp2)) return false;

        if (!IsAdjacents(gp1, gp2)) return false;

        if (!IsSwapTypeValid(gp1, gp2, memberSO)) return false;

        isBusy = true;

        GridObject go1 = GetGridObjectByGridPosition(gp1);
        GridObject go2 = GetGridObjectByGridPosition(gp2);
        Item bufferItem = go1.GetItem();

        GridObjectVisual bufferGo1 = GameObject.Instantiate(itemImageBuffer, go1.GetWorldPosition(), Quaternion.identity);
        GridObjectVisual bufferGo2 = GameObject.Instantiate(itemImageBuffer, go2.GetWorldPosition(), Quaternion.identity);

        bufferGo1.ChangeImage(go1.GetItem());
        bufferGo2.ChangeImage(go2.GetItem());

        go1.ChangeItem(go2.GetItem());
        go2.ChangeItem(bufferItem);

        // go1.CheckMatch();
        // go2.CheckMatch();

        CheckForMatches();

        bufferGo1.MoveTo(bufferGo2.transform.position, () => {
            GameObject.Destroy(bufferGo1.gameObject);
            go1.UpdateVisual();
            isBusy = false;
        });
        bufferGo2.MoveTo(bufferGo1.transform.position, () => {
            GameObject.Destroy(bufferGo2.gameObject);
            go2.UpdateVisual();
            isBusy = false;
            OnEndMove?.Invoke();
        });

        return true;
    }

    private void CheckForMatches() {
        ClearMatches();

        foreach(GridObject go in gridArray) {
            CheckForNeighbors(go);
        }
    }

    private void ClearMatches() {
        foreach(Match match in matches) {
            match.Clear();
        }

        matches.Clear();
    }

    private void CheckForNeighbors(GridObject go) {
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if (x != 0 && y != 0) continue;
                if (x == 0 && y == 0) continue;

                GridPosition neightborGridPosition = new GridPosition(x, y) + go.GetPosition();
                GridObject neighborGridObject = GetGridObjectByGridPosition(neightborGridPosition);

                if (neighborGridObject == null) continue;

                if (neighborGridObject.item != go.item) continue;

                if (go.currentMatch == null) {
                    if (neighborGridObject.currentMatch == null) {
                        go.currentMatch = new Match(go);
                        matches.Add(go.currentMatch);
                        go.currentMatch.AddMatchingObject(neighborGridObject);

                    }
                    else {
                        neighborGridObject.currentMatch.AddMatchingObject(go);
                    }
                } 
                else {
                    if (!go.currentMatch.matchingObjects.Contains(neighborGridObject)) {
                        go.currentMatch.AddMatchingObject(neighborGridObject);
                    }
                }
            }   
        }
    }

    private bool IsSwapTypeValid(GridPosition gp1, GridPosition gp2, MemberSO memberSO) {
        if (memberSO.swapType == SwapType.HORIZONTAL && !IsHorizontal(gp1, gp2)) return false;
        if (memberSO.swapType == SwapType.VERTICAL && !IsVertical(gp1, gp2)) return false;
        if (memberSO.swapType == SwapType.DIAGONAL && !IsDiagonal(gp1, gp2)) return false;
        if (memberSO.swapType == SwapType.BOTH && IsDiagonal(gp1, gp2)) return false;

        return true;
    }

    private bool IsAdjacents(GridPosition gp1, GridPosition gp2) {
        int xDistance = Mathf.Abs(Mathf.Abs(gp1.x) - Mathf.Abs(gp2.x));
        int yDistance = Mathf.Abs(Mathf.Abs(gp1.y) - Mathf.Abs(gp2.y));

        if (xDistance >= 2 || yDistance >= 2) {
            return false;
        }

        return true;
    }

    private bool IsHorizontal(GridPosition gp1, GridPosition gp2) {
        return gp1.y == gp2.y;
    }

    private bool IsVertical(GridPosition gp1, GridPosition gp2) {
        return gp1.x == gp2.x;
    }

    private bool IsDiagonal(GridPosition gp1, GridPosition gp2) {
        return !IsHorizontal(gp1, gp2) && !IsVertical(gp1, gp2);
    }

    private void Regenerate() {
        foreach(GridObject go in gridArray) {
            if (go.GetItem() == null) {
                go.Regenerate(ItemContainer.Instance.GetRandomItem());
            }
        }

        CheckForMatches();
    }

    public async Task ClearMatching() {
        if (isBusy) return;

        DamageManager.Instance.DoDamage(matches.ToArray());

        foreach(Match match in matches) {
            match.Destroy();
        }

        matches.Clear();

        await Task.Delay(1000);

        Regenerate();
    }
}
