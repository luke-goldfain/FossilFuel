using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSliceMovement : MonoBehaviour
{

    private UnityGridManager gridMgrInstance;

    private InputHandler inputHdlr;

    private CharacterTurnInfo charTurnInfo;

    private CameraBillboard billboardScript;

    private Rigidbody rb;

    [SerializeField, Tooltip("The scale to shrink to compared to grid size when in slice mode."), Range(0f, 1f)]
    private float sliceScale;

    [SerializeField]
    private GameObject bazookaPrefab; // TODO: Replace with some kind of loading

    [HideInInspector]
    public GameObject currentWeapon; // TODO: add ability to switch weapons

    private bool firing;
    private bool hasFired;

    private bool turnFinished;

    private float turnEndTime = 4f;
    private float turnEndTimer;

    // Start is called before the first frame update
    void Start()
    {
        inputHdlr = InputHandler.Instance; // TODO: Replace with command pattern

        gridMgrInstance = FindObjectOfType<UnityGridManager>(); // Replace with singleton?

        charTurnInfo = this.gameObject.GetComponent<CharacterTurnInfo>();

        billboardScript = this.gameObject.GetComponent<CameraBillboard>();

        rb = this.gameObject.GetComponent<Rigidbody>();

        currentWeapon = Instantiate(bazookaPrefab, this.transform);

        firing = false;
        hasFired = false;
        turnFinished = false;
        turnEndTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (charTurnInfo.CheckCurrentTurnSegment() == TurnSegments.sliceMovement)
        {
            this.gameObject.transform.localScale = Vector3.Lerp(this.gameObject.transform.localScale, new Vector3(sliceScale, sliceScale, sliceScale), 0.2f);

            if (charTurnInfo.IsMoving)
            {
                UpdateCheckMoveSlice();

                UpdateCheckAdvanceTurn();
            }
        }
    }

    private void UpdateCheckMoveSlice() // TODO: Implement and enforce movement borders
    {
        if (inputHdlr.RightKeyHeld && !firing)
        {
            rb.velocity = this.transform.right;

            billboardScript.IsFlipped = true;
        }

        if (inputHdlr.LeftKeyHeld && !firing)
        {
            rb.velocity = this.transform.right;

            billboardScript.IsFlipped = false;
        }

        if (inputHdlr.UpKeyHeld)
        {
            currentWeapon.GetComponent<BazookaWeapon>().RotateUp(); // TODO: Reference this script abstractly somehow to account for different weapons
        }

        if (inputHdlr.DownKeyHeld)
        {
            currentWeapon.GetComponent<BazookaWeapon>().RotateDown(); // TODO: Reference this script abstractly somehow to account for different weapons
        }

        if (inputHdlr.ChoiceKeyDown && !hasFired)
        {
            hasFired = true;
            firing = true;
        }

        if (inputHdlr.ChoiceKeyHeld && firing)
        {
            currentWeapon.GetComponent<BazookaWeapon>().ChargeShot(); // TODO: Reference this script abstractly somehow to account for different weapons
        }

        if (inputHdlr.ChoiceKeyUp && firing)
        {
            firing = false;
            turnFinished = true;

            currentWeapon.GetComponent<BazookaWeapon>().Fire(); // TODO: Reference this script abstractly somehow to account for different weapons
        }
    }

    private void UpdateCheckAdvanceTurn()
    {
        if (turnFinished)
        {
            turnEndTimer += Time.deltaTime;

            if (turnEndTimer >= turnEndTime)
            {
                charTurnInfo.EndTurn();
            }
        }
    }

    public void RefreshSliceTurn()
    {
        firing = false;
        hasFired = false;
        turnFinished = false;
        turnEndTimer = 0f;
    }
}
