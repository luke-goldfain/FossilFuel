using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour // TODO: Implement command pattern in place of this class's simple booleans
{
    private static InputHandler inputHdlrInstance;

    public static InputHandler Instance
    {
        get
        {
            if (inputHdlrInstance == null)
            {
                GameObject inputGO = new GameObject();
                inputHdlrInstance = inputGO.AddComponent<InputHandler>();
                inputGO.name = "InputHandler (Runtime Singleton)";
            }

            return inputHdlrInstance;
        }
    }

    [HideInInspector]
    public bool UpKeyHeld, UpKeyDown,
                DownKeyHeld, DownKeyDown,
                LeftKeyHeld, LeftKeyDown,
                RightKeyHeld, RightKeyDown,
                ChoiceKeyHeld, ChoiceKeyDown, ChoiceKeyUp;

    // Start is called before the first frame update
    void Start()
    {
        RefreshKeys();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateReflectInput();
    }

    // TODO: Implement command pattern in place of this class's simple booleans
    private void UpdateReflectInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) UpKeyDown = true;
        else UpKeyDown = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) UpKeyHeld = true;
        else UpKeyHeld = false;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) DownKeyDown = true;
        else DownKeyDown = false;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) DownKeyHeld = true;
        else DownKeyHeld = false;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) LeftKeyDown = true;
        else LeftKeyDown = false;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) LeftKeyHeld = true;
        else LeftKeyHeld = false;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) RightKeyDown = true;
        else RightKeyDown = false;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) RightKeyHeld = true;
        else RightKeyHeld = false;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) ChoiceKeyDown = true;
        else ChoiceKeyDown = false;

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) ChoiceKeyHeld = true;
        else ChoiceKeyHeld = false;

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return)) ChoiceKeyUp = true;
        else ChoiceKeyUp = false;
    }

    public void RefreshKeys()
    {
        UpKeyHeld = UpKeyDown =
        DownKeyHeld = DownKeyDown =
        LeftKeyHeld = LeftKeyDown =
        RightKeyHeld = RightKeyDown =
        ChoiceKeyHeld = ChoiceKeyDown = ChoiceKeyUp = false;
    }
}
