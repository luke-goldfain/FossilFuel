﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnSegments { gridMovement, attackSelection, sliceMovement };

public class TurnManager : MonoBehaviour
{
    // observer-ish event handler for switching turns and switching to slice mode
    public delegate void SwitchPlayer();
    public static event SwitchPlayer NotifyOfSwitch;

    private static TurnManager turnMgrInstance;

    public static TurnManager Instance
    {
        get
        {
            if (turnMgrInstance == null)
            {
                GameObject turnGO = new GameObject();
                turnMgrInstance = turnGO.AddComponent<TurnManager>();
                turnGO.name = "TurnManager (Runtime Singleton)";
            }

            return turnMgrInstance;
        }
    }

    private UnityGridManager gridMgr;

    public int MovingPlayer = 1;
    public int MovingChar = 1;

    public GameObject MovingCharInstance;

    public TurnSegments CurrentTurnSegment;

    public void EndTurn(int pNum) // take in player number and switch turn to other player
    {
        if (gridMgr == null)
        {
            gridMgr = FindObjectOfType<UnityGridManager>();
        }

        switch (pNum) // TODO: Add behavior for changing characters
        {
            case 1:
                MovingPlayer = 2;
                EnsureCharIsActive();
                CurrentTurnSegment = TurnSegments.gridMovement;
                break;
            case 2:
                MovingPlayer = 1;
                MovingChar++;
                EnsureCharIsActive();
                CurrentTurnSegment = TurnSegments.gridMovement;
                break;
        }

        NotifyOfSwitch?.Invoke();
    }

    private void EnsureCharIsActive()
    {
        bool charActive = false;

        while (!charActive)
        {
            foreach (GameObject ch in gridMgr.AllChars)
            {
                if (gridMgr.ActiveChars.Contains(ch))
                {
                    CharacterTurnInfo chTurnInfo = ch.GetComponent<CharacterTurnInfo>();

                    if (chTurnInfo.CharacterNumber == MovingChar && chTurnInfo.PlayerNumber == MovingPlayer)
                    {
                        charActive = true;

                        return;
                    }
                }
            }

            if (!charActive)
            {
                MovingChar++;

                if (MovingChar > gridMgr.AllChars.Count / 2)
                {
                    MovingChar = 1;
                }
            }
        }
    }
}
