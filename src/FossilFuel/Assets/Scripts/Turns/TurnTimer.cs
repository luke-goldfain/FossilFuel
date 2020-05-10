using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTimer : MonoBehaviour
{
    private static TurnTimer timerInstance;

    private static readonly object getlock = new object();

    // Monobehaviour singleton
    public static TurnTimer Instance
    {
        get
        {
            lock(getlock)
            {
                if (timerInstance == null)
                {
                    GameObject timerGO = new GameObject();
                    timerInstance = timerGO.AddComponent<TurnTimer>();
                }

                return timerInstance;
            }
        }
    }

    private TurnManager turnMgr;

    public float CurrentTurnTime { get; private set; }

    private void Start()
    {
        turnMgr = TurnManager.Instance;
    }

    private void Update()
    {
        CurrentTurnTime -= Time.deltaTime;

        if (CurrentTurnTime <= 0f && CurrentTurnTime > -1f)
        {
            CurrentTurnTime = -1f;

            turnMgr.EndTurnOfActiveCharacter();
        }
    }

    /// <summary>
    /// Sets the turn time to a value in seconds.
    /// </summary>
    /// <param name="time">The time in seconds to set the turn timer to.</param>
    public void SetTurnTime(float time)
    {
        CurrentTurnTime = time;
    }
}
