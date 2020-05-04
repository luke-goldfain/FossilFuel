using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnTextDisplay : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private TurnManager turnMgr;

    [SerializeField, Tooltip("The in-scene game object for the restart panel, which includes restart-ability.")]
    private GameObject restartPanel;

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

        if (turnMgr.TeamOneActiveChars.Count > 0)
        {
            textMesh.text = "t-rex wins!";
        }
        else
        {
            textMesh.text = "tricera wins!";
        }

        restartPanel.SetActive(true);
    }
}
