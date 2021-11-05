using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{

    public static Action onProblemFixed;
    public static void ProblemFixed()
    {
        onProblemFixed?.Invoke();
    }

}
