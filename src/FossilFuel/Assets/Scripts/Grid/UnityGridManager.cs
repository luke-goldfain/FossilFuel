using Assets.Scripts.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityGridManager : MonoBehaviour
{


    [SerializeField, Tooltip("The prefab for a grid piece")]
    private GameObject gridPiecePrefab;

    private List<GridMovableNode> nodeList;

    private List<GameObject> gridPieceList;

    [SerializeField, Tooltip("The depth/width in grid pieces of the grid")]
    private int gridDepth, gridWidth;

    [SerializeField, Tooltip("The prefab for character one")]
    private GameObject characterOnePrefab;

    [SerializeField, Tooltip("The prefab for character two")]
    private GameObject characterTwoPrefab;

    private GameObject charOneInstance, charTwoInstance;

    private float gridPieceD, gridPieceW;

    // Start is called before the first frame update
    void Start()
    {
        StartCreateGrid();

        StartPlaceCharacters();

        Camera.main.orthographicSize = (gridDepth > gridWidth) ? gridDepth : gridWidth;
    }

    private void StartCreateGrid()
    {
        nodeList = new List<GridMovableNode>();
        gridPieceList = new List<GameObject>();

        gridPieceD = 2.55f; // depth (z-scale) of a grid piece
        gridPieceW = 2.05f; // width (x-scale) of a grid piece

        this.gameObject.transform.position = new Vector3(-(gridPieceW * gridWidth * 0.5f), 0f, -(gridPieceD * gridDepth * 0.5f));

        for (int i = 0; i < gridDepth; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                GameObject currentGridPiece;
                GridMovableNode currentNode = new GridMovableNode(j, i);

                nodeList.Add(currentNode);

                gridPieceList.Add(currentGridPiece = Instantiate(gridPiecePrefab, this.transform.position + new Vector3(((0.5f * gridPieceW) + (j * gridPieceW)), 0f, (0.5f * gridPieceD) + (i * gridPieceD)), Quaternion.identity));

                currentGridPiece.GetComponentInChildren<MovableNodeContainer>().AssignedNode = currentNode;
            }
        }

        foreach (GridMovableNode n1 in nodeList)
        {
            foreach (GridMovableNode n2 in nodeList)
            {
                n2.MatchUpNeighbors(n1);
            }
        }
    }

    private void StartPlaceCharacters()
    {
        charOneInstance = Instantiate(characterOnePrefab, GetNodeContainer(GetNode(0, 1)).gameObject.transform.position, Quaternion.identity);
        charTwoInstance = Instantiate(characterTwoPrefab, GetNodeContainer(GetNode(3, 2)).gameObject.transform.position, Quaternion.identity);

        charOneInstance.GetComponent<CharacterGridMovement>().GridPosX = 0;
        charOneInstance.GetComponent<CharacterGridMovement>().GridPosZ = 1;

        charTwoInstance.GetComponent<CharacterGridMovement>().GridPosX = 3;
        charTwoInstance.GetComponent<CharacterGridMovement>().GridPosZ = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Gets a node at abstracted position x, z in the node list. Defaults to the first node in the list.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public GridMovableNode GetNode(int x, int z)
    {
        GridMovableNode returnNode = null; // default to null

        foreach(GridMovableNode n in nodeList)
        {
            if (n.XValue == x && n.ZValue == z)
            {
                returnNode = n;
            }
        }

        return returnNode;
    }

    /// <summary>
    /// Gets the node container script of a node, allowing Unity things to be done with it.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public MovableNodeContainer GetNodeContainer(GridMovableNode node)
    {
        MovableNodeContainer returnContainer = gridPieceList[0].GetComponentInChildren<MovableNodeContainer>();

        foreach(GameObject cont in gridPieceList)
        {
            if(cont.GetComponentInChildren<MovableNodeContainer>().AssignedNode == node)
            {
                returnContainer = cont.GetComponentInChildren<MovableNodeContainer>();
            }
        }

        return returnContainer;
    }
}
