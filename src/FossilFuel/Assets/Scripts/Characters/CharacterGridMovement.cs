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

    private UnityCharacterTurnInfo charTurnInfo;

    public List<GridMovableNode> MovableNodes;

    private bool movableIdentified;

    private void Awake()
    {
        inputHdlr = InputHandler.Instance; // TODO: Replace with command pattern

        gridMgrInstance = FindObjectOfType<UnityGridManager>(); // Replace with singleton?

        charTurnInfo = this.gameObject.GetComponent<UnityCharacterTurnInfo>();

        MovableNodes = new List<GridMovableNode>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (charTurnInfo.DataCharacter.CheckCurrentTurnSegment() == TurnSegments.gridMovement)
        {
            this.gameObject.transform.localScale = Vector3.Lerp(this.gameObject.transform.localScale, Vector3.one, 0.2f);

            UpdateSetLocationOnNode();

            if (charTurnInfo.DataCharacter.CurrentState == CharacterState.active)
            {
                UpdateCheckMoveGrid();

                UpdateCheckAdvanceTurn();
            }
        }
    }

    /// <summary>
    /// Lock the character's position to the node the player has selected.
    /// </summary>
    private void UpdateSetLocationOnNode()
    {
        this.transform.position = gridMgrInstance.GetNodeContainer(CurrentNode).gameObject.transform.position;
    }

    /// <summary>
    /// Check for input and move the character on the grid accordingly.
    /// </summary>
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

        if (targetNode != null && gridMgrInstance.GetOccupantOfNode(targetNode) == null && MovableNodes.Contains(targetNode))
        {
            GridPosX = targetX;
            GridPosZ = targetZ;

            SetCurrentNode();
        }
    }

    private void UpdateCheckAdvanceTurn()
    {
        if (inputHdlr.ChoiceKeyDown)
        {
            charTurnInfo.AdvanceCurrentTurnSegment();
        }
    }

    public void SetCurrentNode()
    {
        this.CurrentNode = gridMgrInstance.GetNode(GridPosX, GridPosZ);
    }

    public void RefreshGridTurn()
    {
        this.MovableNodes.Clear();
    }
}
