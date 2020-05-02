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

    private Vector3 sliceScaleV3;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private List<GameObject> weaponPrefabsList;

    private List<GameObject> weaponInstancesList;

    [HideInInspector]
    public GameObject CurrentWeapon; // TODO: add ability to switch weapons

    private int currentWeaponIndex;

    [SerializeField, Tooltip("The prefab for boundaries to be placed when in slice mode.")]
    private GameObject boundPrefab;

    private bool oneTimeActionsExecuted;

    private bool firing;
    private bool hasFired;

    private bool onGround;

    private bool turnFinished;

    private float turnEndTime = 7f;
    private float turnEndTimer;

    private CharacterBoundsDisplayer boundsDisp;

    // Start is called before the first frame update
    void Start()
    {
        inputHdlr = InputHandler.Instance; // TODO: Replace with command pattern

        gridMgrInstance = FindObjectOfType<UnityGridManager>(); // Replace with singleton?

        charTurnInfo = this.gameObject.GetComponent<UnityCharacterTurnInfo>();

        billboardScript = this.gameObject.GetComponent<CameraBillboard>();

        rb = this.gameObject.GetComponent<Rigidbody>();

        boundsDisp = new CharacterBoundsDisplayer();

        StartInitWeapons();

        sliceScaleV3 = new Vector3(sliceScale, sliceScale, sliceScale);

        RefreshSliceTurn();
    }

    private void StartInitWeapons()
    {
        weaponInstancesList = new List<GameObject>();

        // Instantiate each of the weapons in the prefab list, adding them to an instances list
        foreach (GameObject w in weaponPrefabsList)
        {
            weaponInstancesList.Add(Instantiate(w, this.transform));
        }

        // Set all except the first weapon inactive
        foreach (GameObject w in weaponInstancesList)
        {
            if (weaponInstancesList.IndexOf(w) != 0)
            {
                w.SetActive(false);
            }
        }

        // Set the current weapon as the active weapon
        CurrentWeapon = weaponInstancesList[0];
        currentWeaponIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (charTurnInfo.DataCharacter.CheckCurrentTurnSegment() == TurnSegments.sliceMovement)
        {
            if (this.gameObject.transform.localScale != sliceScaleV3)
            {
                this.gameObject.transform.localScale = Vector3.Lerp(this.gameObject.transform.localScale, sliceScaleV3, 0.2f);
            }
            else if (!oneTimeActionsExecuted)
            {
                boundsDisp.DisplayBounds(this.gameObject.transform, boundPrefab);

                oneTimeActionsExecuted = true;
            }


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
        AbstractWeapon currentWeaponScript = CurrentWeapon.GetComponent<AbstractWeapon>();

        currentWeaponScript.CrosshairGO.SetActive(true);

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

        if (inputHdlr.ChoiceBackKeyDown && !hasFired)
        {
            currentWeaponIndex--;

            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weaponInstancesList.Count - 1;
            }

            SwitchToWeapon(currentWeaponIndex);
        }

        if (inputHdlr.ChoiceFwdKeyDown && !hasFired)
        {
            currentWeaponIndex++;

            if (currentWeaponIndex >= weaponInstancesList.Count)
            {
                currentWeaponIndex = 0;
            }

            SwitchToWeapon(currentWeaponIndex);
        }

        if (inputHdlr.UpKeyHeld)
        {
           currentWeaponScript.RotateUp(); 
        }

        if (inputHdlr.DownKeyHeld)
        {
            currentWeaponScript.RotateDown(); 
        }

        if (inputHdlr.ChoiceKeyDown && !hasFired)
        {
            hasFired = true;
            firing = true;
        }

        if (inputHdlr.ChoiceKeyHeld && firing)
        {
            currentWeaponScript.ChargeShot(); 
        }

        if (firing && (inputHdlr.ChoiceKeyUp || currentWeaponScript.ShootPower >= currentWeaponScript.MaxShootPower))
        {
            firing = false;
            turnFinished = true;

            currentWeaponScript.Fire(); 
        }
    }

    private void SwitchToWeapon(int index)
    {
        CurrentWeapon.SetActive(false);

        CurrentWeapon = weaponInstancesList[index];

        CurrentWeapon.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 0.3f, ~(1 << 8)))
        {
            onGround = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!inputHdlr.JumpKeyHeld && Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 0.3f, ~(1 << 8)))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 0.3f, ~(1 << 8)))
        {
            onGround = false;
        }
    }

    private void UpdateCheckAdvanceTurn()
    {
        if (turnFinished)
        {
            CurrentWeapon.GetComponent<AbstractWeapon>().CrosshairGO.SetActive(false);

            turnEndTimer += Time.deltaTime;

            if (turnEndTimer >= turnEndTime)
            {
                turnEndTimer = 0f;

                turnFinished = false;

                boundsDisp.RemoveBounds();

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
        oneTimeActionsExecuted = false;
        firing = false;
        hasFired = false;
        onGround = false;
        turnFinished = false;
        turnEndTimer = 0f;
    }
}
