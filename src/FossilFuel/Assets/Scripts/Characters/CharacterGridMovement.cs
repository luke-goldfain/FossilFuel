using Assets.Scripts.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGridMovement : MonoBehaviour
{
    private UnityGridManager gridMgrInstance;

    private InputHandler inputHdlr;

    [SerializeField]
    public int GridPosX, GridPosZ;

    private GridMovableNode currentNode;

    private CharacterTurnInfo charTurnInfo;

    // Start is called before the first frame update
    void Start()
    {
        inputHdlr = InputHandler.Instance; // TODO: Replace with command pattern

        gridMgrInstance = FindObjectOfType<UnityGridManager>(); // Replace with singleton?

        charTurnInfo = this.gameObject.GetComponent<CharacterTurnInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (charTurnInfo.CheckCurrentTurnSegment() == TurnSegments.gridMovement)
        {
            UpdateSetLocationOnNode();

            if (charTurnInfo.IsMoving)
            {
                UpdateCheckMove();

                UpdateCheckAdvanceTurn();
            }
        }
    }

    private void UpdateSetLocationOnNode()
    {
        this.currentNode = gridMgrInstance.GetNode(GridPosX, GridPosZ);
        this.transform.position = gridMgrInstance.GetNodeContainer(currentNode).gameObject.transform.position;
    }

    private void UpdateCheckMove()
    {
        int targetX = GridPosX, targetZ = GridPosZ;

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
            GridPosX = targetX;
            GridPosZ = targetZ;

            currentNode = targetNode;
        }
    }

    private void UpdateCheckAdvanceTurn()
    {
        if (inputHdlr.ChoiceKeyDown)
        {
            charTurnInfo.AdvanceCurrentTurnSegment();
        }
    }
}
