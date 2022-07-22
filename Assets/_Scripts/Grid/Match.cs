using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Match
{
    public List<GridObject> matchingObjects = new();
    public Item item = null;

    public Match(GridObject gridObject) {
        matchingObjects.Add(gridObject);
        item = gridObject.GetItem();
    }

    public void AddMatchingObject(GridObject gridObject) {
        if (matchingObjects.Contains(gridObject)) return;

        matchingObjects.Add(gridObject);
        gridObject.currentMatch = this;
    }

    public void Clear() {
        foreach(GridObject go in matchingObjects) {
            go.currentMatch = null;
        }

        matchingObjects.RemoveAll((go) => go != null);
    }

    public int GetLength() {
        return matchingObjects.Count;
    }

    public void Destroy() {
        if (matchingObjects.Count > 2) {
            foreach(GridObject go in matchingObjects) {
                go.Delete();
            }
        }
        

        Clear();
    }
}
