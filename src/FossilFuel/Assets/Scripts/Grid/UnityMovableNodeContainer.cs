using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Grid;

public class UnityMovableNodeContainer : MonoBehaviour
{
    public GridMovableNode AssignedNode; // Node to be assigned by grid manager

    [SerializeField, Tooltip("The list of visual barrier GameObjects, to be set via prefab.")]
    public List<GameObject> visualBarriers;
    
    public void SetVisualBarriersMaterial(Material m)
    {
        foreach (GameObject b in visualBarriers)
        {
            b.GetComponent<Renderer>().material = m;
        }
    }
}
