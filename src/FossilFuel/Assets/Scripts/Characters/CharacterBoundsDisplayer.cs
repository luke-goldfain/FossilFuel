using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBoundsDisplayer : ScriptableObject
{
    private GameObject bound1, bound2;

    public void DisplayBounds(Transform t, GameObject boundPrefab)
    {
        // First time this is run these will be loaded with the passed in prefab
        if (bound1 == null)
        {
            bound1 = Instantiate(boundPrefab, t.position, Quaternion.identity);
        }

        if (bound2 == null)
        {
            bound2 = Instantiate(boundPrefab, t.position, Quaternion.identity);
        }

        // Raycast 2 units to the right and left of the passed transform (a character's transform).
        if (Physics.Raycast(t.position, t.right, out RaycastHit hitR, 2f, (1 << 8)))
        {
            // If hit (which should never be false), put the boundary game object there. 
            bound1.transform.position = hitR.point;

            bound1.SetActive(true);
        }

        if (Physics.Raycast(t.position, -t.right, out RaycastHit hitL, 2f, (1 << 8)))
        {
            bound2.transform.position = hitL.point;

            bound2.SetActive(true);
        }
    }

    public void RemoveBounds()
    {
        bound1.SetActive(false);
        bound2.SetActive(false);
    }
}
