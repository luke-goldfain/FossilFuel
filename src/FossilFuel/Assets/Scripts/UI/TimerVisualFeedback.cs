using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerVisualFeedback : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Color lowTimeColor;

    private TurnTimer turnTimer;

    private void Start()
    {
        turnTimer = TurnTimer.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = ((int)turnTimer.CurrentTurnTime).ToString();

        if (turnTimer.CurrentTurnTime < 10f)
        {
            timerText.color = lowTimeColor;
        }
        else if (timerText.color != Color.white)
        {
            timerText.color = Color.white;
        }
    }
}
