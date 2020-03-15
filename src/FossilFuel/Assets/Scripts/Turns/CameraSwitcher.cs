using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private TurnManager turnMgr;

    private UnityGridManager gridMgr;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [SerializeField, Tooltip("The speed at which the camera lerps between vertical slice and top-down views."), Range(0f, 0.2f)]
    private float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        turnMgr = TurnManager.Instance;
        gridMgr = FindObjectOfType<UnityGridManager>();

        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnMgr.CurrentTurnSegment == TurnSegments.sliceMovement)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, (turnMgr.MovingCharInstance.transform.position + turnMgr.MovingCharInstance.GetComponent<CharacterTurnInfo>().AttackTarget.transform.position) / 2 - this.transform.forward, lerpSpeed);

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(Vector3.Cross(Vector3.up, turnMgr.MovingCharInstance.transform.position - turnMgr.MovingCharInstance.GetComponent<CharacterTurnInfo>().AttackTarget.transform.position), Vector3.up), lerpSpeed);
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, originalPosition, lerpSpeed);

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, originalRotation, lerpSpeed);
        }
    }
}
