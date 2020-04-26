using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSliceMovement : MonoBehaviour
{

    private UnityGridManager gridMgrInstance;

    private InputHandler inputHdlr;

    private UnityCharacterTurnInfo charTurnInfo;

    private CameraBillboard billboardScript;

    private Rigidbody rb;

    [SerializeField, Tooltip("The scale to shrink to compared to grid size when in slice mode."), Range(0f, 1f)]
    private float sliceScale;

    [SerializeField]
    private GameObject bazookaPrefab; // TODO: Replace with some kind of loading

    [HideInInspector]
    public GameObject currentWeapon; // TODO: add ability to switch weapons

    [SerializeField]
    private float jumpForce = 5f;

    private bool firing;
    private bool hasFired;

    private bool onGround;

    private bool turnFinished;

    private float turnEndTime = 4f;
    private float turnEndTimer;

    // Start is called before the first frame update
    void Start()
    {
        inputHdlr = InputHandler.Instance; // TODO: Replace with command pattern

        gridMgrInstance = FindObjectOfType<UnityGridManager>(); // Replace with singleton?

        charTurnInfo = this.gameObject.GetComponent<UnityCharacterTurnInfo>();

        billboardScript = this.gameObject.GetComponent<CameraBillboard>();

        rb = this.gameObject.GetComponent<Rigidbody>();

        currentWeapon = Instantiate(bazookaPrefab, this.transform);

        firing = false;
        hasFired = false;
        onGround = false;
        turnFinished = false;
        turnEndTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (charTurnInfo.DataCharacter.CheckCurrentTurnSegment() == TurnSegments.sliceMovement)
        {
            this.gameObject.transform.localScale = Vector3.Lerp(this.gameObject.transform.localScale, new Vector3(sliceScale, sliceScale, sliceScale), 0.2f);

            if (charTurnInfo.DataCharacter.CurrentState == CharacterState.active)
            {
                UpdateCheckMoveSlice();
            }

            if (turnFinished)
            {
                UpdateCheckAdvanceTurn();
            }
        }
    }

    private void UpdateCheckMoveSlice() 
    {
        currentWeapon.GetComponent<AbstractWeapon>().CrosshairGO.SetActive(true);

        if (inputHdlr.RightKeyHeld && !firing && onGround)
        {
            rb.velocity = this.transform.right;

            billboardScript.IsFlipped = true;
        }

        if (inputHdlr.LeftKeyHeld && !firing && onGround)
        {
            rb.velocity = this.transform.right;

            billboardScript.IsFlipped = false;
        }

        if (inputHdlr.JumpKeyDown && !firing && onGround)
        {
            rb.velocity += Vector3.up * jumpForce;

            onGround = false;
        }

        if (inputHdlr.UpKeyHeld)
        {
            currentWeapon.GetComponent<AbstractWeapon>().RotateUp(); // TODO: Reference this script abstractly somehow to account for different weapons
        }

        if (inputHdlr.DownKeyHeld)
        {
            currentWeapon.GetComponent<AbstractWeapon>().RotateDown(); // TODO: Reference this script abstractly somehow to account for different weapons
        }

        if (inputHdlr.ChoiceKeyDown && !hasFired)
        {
            hasFired = true;
            firing = true;
        }

        if (inputHdlr.ChoiceKeyHeld && firing)
        {
            currentWeapon.GetComponent<AbstractWeapon>().ChargeShot(); // TODO: Reference this script abstractly somehow to account for different weapons
        }

        if (inputHdlr.ChoiceKeyUp && firing)
        {
            firing = false;
            turnFinished = true;

            currentWeapon.GetComponent<AbstractWeapon>().Fire(); // TODO: Reference this script abstractly somehow to account for different weapons
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 0.3f, ~(1 << 8)))
        {
            onGround = true;
        }
    }

    private void UpdateCheckAdvanceTurn()
    {
        if (turnFinished)
        {
            currentWeapon.GetComponent<AbstractWeapon>().CrosshairGO.SetActive(false);

            turnEndTimer += Time.deltaTime;

            if (turnEndTimer >= turnEndTime)
            {
                turnEndTimer = 0f;

                turnFinished = false;

                // Here is where the turn ends
                charTurnInfo.DataCharacter.EndTurn();
            }
        }
    }

    public void ForceFinishSliceTurn()
    {
        firing = false;
        turnFinished = true;
    }

    public void RefreshSliceTurn()
    {
        firing = false;
        hasFired = false;
        onGround = false;
        turnFinished = false;
        turnEndTimer = 0f;
    }
}
