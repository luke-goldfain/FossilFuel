using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityGridManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gridPiecePrefab;

    

    [SerializeField, Tooltip("The depth/width in grid pieces of the grid")]
    private int gridDepth, gridWidth;

    private float gridPieceD, gridPieceW;

    // Start is called before the first frame update
    void Start()
    {
        gridPieceD = 2.55f; // depth (z-scale) of a grid piece
        gridPieceW = 2.05f; // width (x-scale) of a grid piece

        this.gameObject.transform.position = new Vector3(-(gridPieceW * gridWidth * 0.5f), 0f, -(gridPieceD * gridDepth * 0.5f));

        for(int i = 0; i < gridDepth; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                Instantiate(gridPiecePrefab, this.transform.position + new Vector3(((0.5f * gridPieceW) + (j * gridPieceW)), 0f, (0.5f * gridPieceD) + (i * gridPieceD)), Quaternion.identity);
            }
        }

        Camera.main.orthographicSize = (gridDepth > gridWidth) ? gridDepth : gridWidth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
