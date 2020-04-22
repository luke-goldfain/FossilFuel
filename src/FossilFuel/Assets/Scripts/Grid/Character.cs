using UnityEngine;

public enum CharacterState
{
    inactive, active, dead
}

public class Character
{
    private TurnManager turnMgr;

    public int PlayerNumber;

    public CharacterState CurrentState;

    public GameObject CharGO;

    public bool WentThisTurn;

    public Character(int pNum, GameObject go)
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
    }

    public TurnSegments CheckCurrentTurnSegment()
    {
        return turnMgr.CurrentTurnSegment;
    }
}