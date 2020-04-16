using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnTextDisplay : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private TurnManager turnMgr;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = this.GetComponent<TextMeshProUGUI>();

        turnMgr = TurnManager.Instance;

        TurnManager.NotifyOfSwitch += UpdateTurnText;
        TurnManager.NotifyOfGameEnd += DisplayWinner;
    }

    public void UpdateTurnText()
    {
        switch (turnMgr.MovingPlayer)
        {
            case 1:
                textMesh.text = "turn: t-rex";
                break;
            case 2:
                textMesh.text = "turn: tricera";
                break;
        }
    }

    public void DisplayWinner()
    {
        TurnManager.NotifyOfSwitch -= UpdateTurnText;

        switch (turnMgr.MovingPlayer)
        {
            case 1:
                textMesh.text = "T-rex wins!";
                break;
            case 2:
                textMesh.text = "Tricera wins!";
                break;
        }
    }
}
