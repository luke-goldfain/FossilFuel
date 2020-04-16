using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnSegments { gridMovement, attackSelection, sliceMovement };

public class TurnManager : MonoBehaviour
{
    // observer-ish event handler for switching turns and switching to slice mode
    public delegate void SwitchPlayer();
    public static event SwitchPlayer NotifyOfSwitch;

    // observer-ish event handler for a team winning
    public delegate void EndGame();
    public static event EndGame NotifyOfGameEnd;

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

        // Notify all observers of NotifyOfSwitch that the player has been switched
        NotifyOfSwitch?.Invoke();

        CheckBothTeamsForActive();
    }

    /// <summary>
    /// Ensures that the character TurnManager is attempting to give control to is active, iterating through the active characters list until control is given successfully.
    /// </summary>
    private void EnsureCharIsActive()
    {
        bool charActive = false;

        int initiallyCheckedChar = MovingChar;

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
                else
                {
                    // Set characters inactive that are no longer in the ActiveChars list
                    ch.SetActive(false); // TODO: Add gravestone, maybe death animation
                }
            }

            // Iterate thru the list if we haven't found an active character yet
            if (!charActive)
            {
                MovingChar++;

                if (MovingChar > gridMgr.AllChars.Count / 2)
                {
                    MovingChar = 1;
                }

                if (MovingChar == initiallyCheckedChar) 
                {
                    // Placeholder switch to other player
                    MovingPlayer = MovingPlayer == 1 ? 2 : 1;

                    // Notify all observers of NotifyOfGameEnd that a player has won, and the game is over
                    NotifyOfGameEnd?.Invoke();
                }
            }
        }
    }

    /// <summary>
    /// Checks whether both teams still have an active character. If they don't, notifies subscribers that the game is over.
    /// </summary>
    private void CheckBothTeamsForActive()
    {
        bool bothTeamsHaveActive = false;
        bool teamOneActive = false;
        bool teamTwoActive = false;

        foreach(GameObject ch in gridMgr.ActiveChars)
        {
            switch (ch.GetComponent<CharacterTurnInfo>().CharacterNumber)
            {
                case 1:
                    teamOneActive = true;
                    break;
                case 2:
                    teamTwoActive = true;
                    break;
            }

            if (teamOneActive && teamTwoActive)
            {
                bothTeamsHaveActive = true;

                return;
            }
        }

        if (!bothTeamsHaveActive)
        {
            NotifyOfGameEnd?.Invoke();
        }
    }
}
