using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : Singleton<TurnSystem>
{
    private int turnCount = 1;
    private bool isPlayerTurn = true;

    public event Action<int, bool> OnTurnChanged = null;

    public void NextTurn() {
        isPlayerTurn = !isPlayerTurn;
        if (isPlayerTurn) {
            turnCount += 1;
        }

        OnTurnChanged?.Invoke(turnCount, isPlayerTurn);
    }

    public bool IsPlayerTurn() {
        return isPlayerTurn;
    }
}
