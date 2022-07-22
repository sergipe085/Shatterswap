using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : Singleton<MouseWorld>
{
    public Vector2 GetPosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
