using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    private TurnManager turnMgr;

    private Camera mainCam;

    private Vector3 sliceTargetDirection;

    [HideInInspector]
    public bool IsFlipped;

    // Start is called before the first frame update
    void Start()
    {
        turnMgr = TurnManager.Instance;

        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnMgr.CurrentTurnSegment == TurnSegments.sliceMovement)
        {
            if (IsFlipped)
            {
                this.transform.rotation = Quaternion.LookRotation(mainCam.transform.forward, Vector3.up);
            }
            else
            {
                this.transform.rotation = Quaternion.LookRotation(-mainCam.transform.forward, Vector3.up);
            }
        }
        else
        {
            this.transform.rotation = Quaternion.LookRotation(this.transform.position - mainCam.transform.position, Vector3.Cross(this.transform.position - mainCam.transform.position, Vector3.right));
        }
    }
}
