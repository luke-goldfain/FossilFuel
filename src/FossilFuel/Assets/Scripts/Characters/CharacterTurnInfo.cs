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

    [HideInInspector]
    public GameObject AttackTarget;

    private void Awake()
    {
        turnMgr = TurnManager.Instance;

        TurnManager.NotifyOfSwitch += StartTurn;

        // Set defaults at start of game and start the turn of player 1 character 1
        if (PlayerNumber == 1)
        {
            turnMgr.MovingPlayer = this.PlayerNumber;
            turnMgr.MovingChar = 1;

            if (turnMgr.MovingChar == this.CharacterNumber)
            {
                StartTurn();
            }
        }
    }

    public void StartTurn()
    {
        IsMoving = true;

        turnMgr.MovingCharInstance = this.gameObject;
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

        // HACK: InputHandler update isn't being called before the next time it's asked for ChoiceKeyDown,
        // so this forces that false.
        FindObjectOfType<InputHandler>().ChoiceKeyDown = false;
    }
}
