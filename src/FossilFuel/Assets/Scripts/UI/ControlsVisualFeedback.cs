using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsVisualFeedback : MonoBehaviour
{
    private InputHandler inputHdlr;

    private TurnManager turnMgr;

    [SerializeField]
    private Color keyDownColor, keyUpColor;

    [SerializeField]
    private Image spaceBar, upArrow, downArrow, leftArrow, rightArrow, attackFill;

    // Start is called before the first frame update
    void Start()
    {
        inputHdlr = InputHandler.Instance;
        turnMgr = TurnManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHdlr.ChoiceKeyHeld)
        {
            spaceBar.color = keyDownColor;

            if (turnMgr.CurrentTurnSegment == TurnSegments.sliceMovement)
            {
                BazookaWeapon weapon = turnMgr.MovingCharInstance.GetComponentInChildren<BazookaWeapon>(); // TODO: Replace with an abstracted call to AbstractWeapon somehow

                attackFill.fillAmount += weapon.ShootPowerIncrement / weapon.MaxShootPower;
            }
            else
            {
                attackFill.fillAmount = 0f;
            }
        }
        else
        {
            spaceBar.color = keyUpColor;
            attackFill.fillAmount = 0f;
        }

        if (inputHdlr.UpKeyHeld)
        {
            upArrow.color = keyDownColor;
        }
        else
        {
            upArrow.color = keyUpColor;
        }

        if (inputHdlr.DownKeyHeld)
        {
            downArrow.color = keyDownColor;
        }
        else
        {
            downArrow.color = keyUpColor;
        }

        if (inputHdlr.LeftKeyHeld)
        {
            leftArrow.color = keyDownColor;
        }
        else
        {
            leftArrow.color = keyUpColor;
        }

        if (inputHdlr.RightKeyHeld)
        {
            rightArrow.color = keyDownColor;
        }
        else
        {
            rightArrow.color = keyUpColor;
        }
    }
}
