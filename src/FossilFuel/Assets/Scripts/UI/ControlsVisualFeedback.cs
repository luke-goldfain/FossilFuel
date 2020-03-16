using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsVisualFeedback : MonoBehaviour
{
    private InputHandler inputHdlr;

    [SerializeField]
    private Color keyDownColor, keyUpColor;

    [SerializeField]
    private Image spaceBar, upArrow, downArrow, leftArrow, rightArrow;

    // Start is called before the first frame update
    void Start()
    {
        inputHdlr = InputHandler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHdlr.ChoiceKeyHeld)
        {
            spaceBar.color = keyDownColor;
        }
        else
        {
            spaceBar.color = keyUpColor;
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
