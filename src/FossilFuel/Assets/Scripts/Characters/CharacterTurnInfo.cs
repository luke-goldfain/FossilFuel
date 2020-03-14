using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurnInfo : MonoBehaviour
{
    private TurnManager turnMgr;

    [SerializeField, Tooltip("The player number for this character. Determines what team it's on, inputability, etc.")]
    public int PlayerNumber;

    [SerializeField, Tooltip("The number for this character. Determines turn order and starting position.")]
    public int CharacterNumber;

    [HideInInspector]
    public bool IsMoving;

    private void Awake()
    {
        turnMgr = TurnManager.Instance;

        TurnManager.NotifyOfSwitch += StartTurn;

        if (PlayerNumber == 1)
        {
            turnMgr.MovingPlayer = this.PlayerNumber;

            StartTurn();
        }
    }

    public void StartTurn()
    {
        IsMoving = true;
    }

    public void EndTurn()
    {
        IsMoving = false;

        turnMgr.EndTurn(this.PlayerNumber);
    }

    public TurnSegments CheckCurrentTurnSegment()
    {
        return turnMgr.CurrentTurnSegment;
    }

    public void AdvanceCurrentTurnSegment()
    {
        turnMgr.CurrentTurnSegment++;
    }
}
