using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this as T;
        }
    }
}
