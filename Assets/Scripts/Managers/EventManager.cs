using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    private void Awake()
    {
        onProblemFixed = null;
        onRoundStart = null;
        onRoundEnd = null;
    }

    public static Action onProblemFixed;
    public static void ProblemFixed()
    {
        onProblemFixed?.Invoke();
    }

    public static Action onRoundStart;
    public static void StartRound()
    {
        onRoundStart?.Invoke();
    }

    public static Action<bool> onRoundEnd;
    public static void EndRound(bool win)
    {
        onRoundEnd?.Invoke(win);
    }

}
