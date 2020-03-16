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

    public GridMovableNode CurrentNode;

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
            this.gameObject.transform.localScale = Vector3.Lerp(this.gameObject.transform.localScale, Vector3.one, 0.2f);

            UpdateSetLocationOnNode();

            if (charTurnInfo.IsMoving)
            {
                UpdateCheckMoveGrid();

                UpdateCheckAdvanceTurn();
            }
        }
    }

    private void UpdateSetLocationOnNode()
    {
        this.CurrentNode = gridMgrInstance.GetNode(GridPosX, GridPosZ);
        this.transform.position = gridMgrInstance.GetNodeContainer(CurrentNode).gameObject.transform.position;
    }

    private void UpdateCheckMoveGrid()
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

        if (targetNode != null && gridMgrInstance.GetOccupantOfNode(targetNode) == null)
        {
            GridPosX = targetX;
            GridPosZ = targetZ;

            CurrentNode = targetNode;
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
