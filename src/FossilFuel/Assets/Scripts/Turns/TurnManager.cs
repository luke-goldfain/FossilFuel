using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnSegments { gridMovement, attackSelection, sliceMovement };

public class TurnManager
{
    // observer-ish event handler for switching turns and switching to slice mode
    public delegate void SwitchPlayer();
    public static event SwitchPlayer NotifyOfSwitch;

    // observer-ish event handler for a team winning
    public delegate void EndGame();
    public static event EndGame NotifyOfGameEnd;

    public List<TurnCharacter> ActiveCharacters { get; protected set; }
    public List<TurnCharacter> TeamOneActiveChars { get; protected set; }
    public List<TurnCharacter> TeamTwoActiveChars { get; protected set; }

    private static TurnManager turnMgrInstance;

    private static readonly object getlock = new object();

    public static TurnManager Instance
    {
        get
        {
            lock (getlock)
            {
                if (turnMgrInstance == null) // This may be able to be refactored to not require a GameObject
                {
                    turnMgrInstance = new TurnManager();
                }

                return turnMgrInstance;
            }
        }
    }

    private UnityGridManager gridMgr;
    
    public int MovingPlayer = 1;

    public int MovingChar = 0;

    public GameObject MovingCharInstance;

    public TurnSegments CurrentTurnSegment { get; private set; }

    public void EndTurn(TurnCharacter ch) // take in a character and find a character on the opposite team to activate
    {
        PruneDeadCharacters();

        // Check for characters that haven't gone this turn
        bool allCharsWent = true;

        foreach(TurnCharacter wCh in ActiveCharacters)
        {
            if (!wCh.WentThisTurn)
            {
                allCharsWent = false;
            }
        }

        if (allCharsWent) // Start a new turn! (may add more here later)
        {
            // As this is a new turn, set all chars' WentThisTurn to false
            foreach (TurnCharacter tCh in ActiveCharacters)
            {
                tCh.WentThisTurn = false;
            }
        }

        StartNextCharacterTurn(ch);

        // Notify all observers of NotifyOfSwitch that the player has been switched
        NotifyOfSwitch?.Invoke();

        CheckBothTeamsForActive();
    }

    private void PruneDeadCharacters()
    {
        List<TurnCharacter> charsToRemove = new List<TurnCharacter>();
        List<GameObject> charsGOToRemove = new List<GameObject>();

        foreach (TurnCharacter dCh in ActiveCharacters)
        {
            if (dCh.CurrentState == CharacterState.dead)
            {
                switch (dCh.PlayerNumber)
                {
                    case 1:
                        TeamOneActiveChars.Remove(dCh);
                        break;
                    case 2:
                        TeamTwoActiveChars.Remove(dCh);
                        break;
                }

                // TODO: Death animation, gravestone, etc?
                dCh.CharGO.SetActive(false);

                charsToRemove.Add(dCh);
                charsGOToRemove.Add(dCh.CharGO);
            }
        }

        foreach (TurnCharacter rCh in charsToRemove)
        {
            ActiveCharacters.Remove(rCh);
            gridMgr.ActiveCharsGO.Remove(rCh.CharGO); // Really should move ActiveCharsGO to here probably
        }

        charsToRemove.Clear();
    }

    private void StartNextCharacterTurn(TurnCharacter ch)
    {
        int checkIndex = ActiveCharacters.IndexOf(ch) + 1;

        if (checkIndex >= ActiveCharacters.Count)
        {
            checkIndex = 0;
        }

        TurnCharacter lastChar = ch;

        // Get the next non-dead character with opposite player, pruning any dead ones.
        // Give up if we've cycled back to the character that just acted, or if a team is empty. Once CheckBothTeamsForActive() is called, the game will know to end.
        while (ActiveCharacters[checkIndex] != lastChar && TeamOneActiveChars.Count > 0 && TeamTwoActiveChars.Count > 0)
        {
            if (checkIndex < ActiveCharacters.Count)
            {
                if (ActiveCharacters[checkIndex].CurrentState != CharacterState.dead && ActiveCharacters[checkIndex].PlayerNumber != this.MovingPlayer && !ActiveCharacters[checkIndex].WentThisTurn)
                {
                    StartTurnByNumber(checkIndex);

                    break;
                }
                else if (ActiveCharacters[checkIndex].CurrentState == CharacterState.dead) // Kill character if their CurrentState is "dead"
                {
                    switch (ActiveCharacters[checkIndex].PlayerNumber)
                    {
                        case 1:
                            TeamOneActiveChars.Remove(ActiveCharacters[checkIndex]);
                            break;
                        case 2:
                            TeamTwoActiveChars.Remove(ActiveCharacters[checkIndex]);
                            break;
                    }

                    // TODO: Death animation, gravestone, etc?
                    ActiveCharacters[checkIndex].CharGO.SetActive(false);

                    ActiveCharacters.Remove(ActiveCharacters[checkIndex]);
                }
                else if (ActiveCharacters[checkIndex].CurrentState != CharacterState.dead && (ActiveCharacters[checkIndex].PlayerNumber == this.MovingPlayer || ActiveCharacters[checkIndex].WentThisTurn)) // Skip over the last-moving player's characters, and characters that went this turn
                {
                    checkIndex++;
                }
            }

            if (checkIndex >= ActiveCharacters.Count)
            {
                checkIndex = 0;
            }
        }
    }

    public void StartTurnByNumber(int charNum)
    {
        CurrentTurnSegment = TurnSegments.gridMovement;
        
        gridMgr.SetAllNodeMatsActive(true);

        ActiveCharacters[charNum].StartTurn();

        ActiveCharacters[charNum].CharGO.GetComponent<CharacterGridMovement>().SetCurrentNode(); // Ensure char's current node is set
        ActiveCharacters[charNum].CharGO.GetComponent<CharacterGridMovement>().RefreshGridTurn();
        gridMgr.IdentifyMovableNodes(ActiveCharacters[charNum]); // this has a functional dependency on the queried character having a "CurrentNode"

        MovingPlayer = ActiveCharacters[charNum].PlayerNumber;

        MovingCharInstance = ActiveCharacters[charNum].CharGO;
    }

    /// <summary>
    /// Checks whether both teams still have an active character. If they don't, notifies subscribers that the game is over.
    /// </summary>
    private void CheckBothTeamsForActive()
    {
        if (TeamOneActiveChars.Count == 0 || TeamTwoActiveChars.Count == 0)
        {
            // Notify all observers of NotifyOfGameEnd that a player has won, and the game is over
            NotifyOfGameEnd?.Invoke();
        }
    }

    public void InitCharacterList()
    {
        ActiveCharacters = new List<TurnCharacter>();
        TeamOneActiveChars = new List<TurnCharacter>();
        TeamTwoActiveChars = new List<TurnCharacter>();
    }

    public void AssignGridManager(UnityGridManager gm)
    {
        this.gridMgr = gm;
    }

    public TurnCharacter CreateActiveCharacter(int pNum, GameObject go)
    {
        TurnCharacter ch = new TurnCharacter(pNum, go);

        ActiveCharacters.Add(ch);

        switch (pNum)
        {
            case 1:
                TeamOneActiveChars.Add(ch);
                break;
            case 2:
                TeamTwoActiveChars.Add(ch);
                break;
        }

        return ch;
    }

    public void AdvanceCurrentTurnSegment()
    {
        CurrentTurnSegment++;

        switch (CurrentTurnSegment)
        {
            case (TurnSegments.attackSelection):

                break;
            case (TurnSegments.sliceMovement):
                gridMgr.SetAllNodeMatsActive(false);
                break;
        }
    }
}

///// <summary>
///// Ensures that the character TurnManager is attempting to give control to is active, iterating through the active characters list until control is given successfully.
///// </summary>
//private void EnsureCharIsActive()
//{
//    bool charActive = false;

//    int initiallyCheckedChar = MovingChar;

//    while (!charActive)
//    {
//        foreach (GameObject ch in gridMgr.AllCharsGO)
//        {
//            if (gridMgr.ActiveCharsGO.Contains(ch))
//            {
//                UnityCharacterTurnInfo chTurnInfo = ch.GetComponent<UnityCharacterTurnInfo>();

//                if (chTurnInfo.CharacterNumber == MovingChar && chTurnInfo.PlayerNumber == MovingPlayer)
//                {
//                    charActive = true;

//                    return;
//                }
//            }
//            else
//            {
//                // Set characters inactive that are no longer in the ActiveChars list
//                ch.SetActive(false); // TODO: Add gravestone, maybe death animation
//            }
//        }

//        // Iterate thru the list if we haven't found an active character yet
//        if (!charActive)
//        {
//            MovingChar++;

//            if (MovingChar > gridMgr.AllCharsGO.Count / 2)
//            {
//                MovingChar = 1;
//            }

//            if (MovingChar == initiallyCheckedChar) 
//            {
//                // Placeholder switch to other player
//                MovingPlayer = MovingPlayer == 1 ? 2 : 1;

//                // Notify all observers of NotifyOfGameEnd that a player has won, and the game is over
//                NotifyOfGameEnd?.Invoke();
//            }
//        }
//    }
//}
