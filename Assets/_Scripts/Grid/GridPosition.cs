using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int y;

    public GridPosition(int _x, int _y) {
        this.x = _x;
        this.y = _y;
    }

    public bool Equals(GridPosition other) {
        return this.x == other.x && this.y == other.y;
    }

    public static bool operator==(GridPosition gp1, GridPosition gp2) {
        return gp1.x == gp2.x && gp1.y == gp2.y;
    }

    public static bool operator!=(GridPosition gp1, GridPosition gp2) {
        return gp1.x != gp2.x || gp1.y != gp2.y;
    }

    public static GridPosition operator+(GridPosition gp1, GridPosition gp2) {
        return new GridPosition(gp1.x + gp2.x, gp1.y + gp2.y);
    }

    public override string ToString() {
        return "x: " + x + "; y: " + y;
    }

    public static GridPosition Null() {
        return new GridPosition(-1, -1);
    }
}
