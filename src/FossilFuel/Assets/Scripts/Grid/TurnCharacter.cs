using UnityEngine;

public enum CharacterState
{
    inactive, active, dead
}

public class TurnCharacter
{
    private TurnManager turnMgr;

    public int PlayerNumber;

    public CharacterState CurrentState;

    public GameObject CharGO;

    public bool WentThisTurn;

    public TurnCharacter(int pNum, GameObject go)
    {
        PlayerNumber = pNum;

        CharGO = go;

        // All the things to set when this is created
        WentThisTurn = false;

        turnMgr = TurnManager.Instance;

        CharGO.GetComponent<UnityCharacterTurnInfo>().DataCharacter = this;
    }

    public void StartTurn()
    {
        CurrentState = CharacterState.active;
    }

    public void EndTurn()
    {
        if (CurrentState != CharacterState.dead)
        {
            CurrentState = CharacterState.inactive;
        }

        WentThisTurn = true;

        turnMgr.EndTurn(this);
    }

    public void Die()
    {
        CurrentState = CharacterState.dead;

        CharGO.GetComponent<CharacterSliceMovement>().ForceFinishSliceTurn(); // in case the character died without firing on their own turn, force the turn to end
    }

    public TurnSegments CheckCurrentTurnSegment()
    {
        return turnMgr.CurrentTurnSegment;
    }
}