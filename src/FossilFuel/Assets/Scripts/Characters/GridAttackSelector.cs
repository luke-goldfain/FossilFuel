using Assets.Scripts.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAttackSelector : MonoBehaviour
{
    private UnityGridManager gridMgrInstance;

    private InputHandler inputHdlr;

    public GameObject AttackCursorPrefab;
    private GameObject attackCursorInstance;

    [HideInInspector]
    public int CursorGridPosX, CursorGridPosZ;

    private GridMovableNode cursorCurrentNode;

    private CharacterTurnInfo charTurnInfo;
    
    void Start()
    {
        inputHdlr = InputHandler.Instance; // TODO: Replace with command pattern

        gridMgrInstance = FindObjectOfType<UnityGridManager>(); // Replace with singleton?

        charTurnInfo = this.gameObject.GetComponent<CharacterTurnInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (charTurnInfo.CheckCurrentTurnSegment() == TurnSegments.attackSelection)
        {
            if (charTurnInfo.IsMoving)
            {
                // Instantiate attack cursor as soon as this segment starts
                if (attackCursorInstance == null)
                {
                    attackCursorInstance = Instantiate(AttackCursorPrefab, this.gameObject.transform.position, Quaternion.identity);

                    CursorGridPosX = this.gameObject.GetComponent<CharacterGridMovement>().GridPosX;
                    CursorGridPosZ = this.gameObject.GetComponent<CharacterGridMovement>().GridPosZ;
                }

                UpdateSetCursorLocationOnNode();

                UpdateCheckMoveCursor();

                UpdateCheckAdvanceTurn();
            }
        }
    }

    private void UpdateSetCursorLocationOnNode()
    {
        this.cursorCurrentNode = gridMgrInstance.GetNode(CursorGridPosX, CursorGridPosZ);
        attackCursorInstance.transform.position = gridMgrInstance.GetNodeContainer(cursorCurrentNode).gameObject.transform.position;
    }

    private void UpdateCheckMoveCursor()
    {
        int targetX = CursorGridPosX, targetZ = CursorGridPosZ;

        GridMovableNode targetNode;

        if (inputHdlr.UpKeyDown)
        {
            targetZ++;
        }
        if (inputHdlr.DownKeyDown)
        {
            targetZ--;
        }
        if (inputHdlr.LeftKeyDown)
        {
            targetX--;
        }
        if (inputHdlr.RightKeyDown)
        {
            targetX++;
        }

        targetNode = gridMgrInstance.GetNode(targetX, targetZ);

        if (targetNode != null)
        {
            CursorGridPosX = targetX;
            CursorGridPosZ = targetZ;

            cursorCurrentNode = targetNode;
        }
    }

    private void UpdateCheckAdvanceTurn()
    {
        // Advance if the player selects a node occupied by a dino of the opposite team
        if (inputHdlr.ChoiceKeyDown && gridMgrInstance.CheckIfNodeOccupied(cursorCurrentNode) && gridMgrInstance.GetOccupantOfNode(cursorCurrentNode).GetComponent<CharacterTurnInfo>().PlayerNumber != charTurnInfo.PlayerNumber)
        {
            charTurnInfo.AttackTarget = gridMgrInstance.GetOccupantOfNode(cursorCurrentNode);

            Destroy(attackCursorInstance);
            attackCursorInstance = null;

            this.GetComponent<CharacterSliceMovement>().RefreshSliceTurn(); // Should maybe do this in a "turn segment starting" thing in charTurnInfo instead

            charTurnInfo.AdvanceCurrentTurnSegment();
        }
    }
}
