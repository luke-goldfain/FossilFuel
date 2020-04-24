using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCharacterTurnInfo : MonoBehaviour
{

    [SerializeField, Tooltip("The player number for this character. Determines what team it's on, inputability, etc.")]
    public int PlayerNumber;

    [SerializeField, Tooltip("The number for this character. Determines turn order and starting position.")]
    public int CharacterNumber;

    [HideInInspector]
    public GameObject AttackTarget;

    public TurnCharacter DataCharacter;

    private TurnManager turnMgr;

    private void Awake()
    {
        turnMgr = TurnManager.Instance;
    }

    public void AdvanceCurrentTurnSegment()
    {
        turnMgr.CurrentTurnSegment++;

        // HACK: InputHandler update isn't being called before the next time it's asked for ChoiceKeyDown and the rest,
        // so this forces that false.
        FindObjectOfType<InputHandler>().RefreshKeys();
    }
}
