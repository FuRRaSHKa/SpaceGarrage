using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class ProblemScript : MonoBehaviour
{
    [SerializeField] private Instrument targetInstrumentType;
    [SerializeField] private Transform placeForFixing;
    [SerializeField] private float fixingTime;
    
    private IDisposable timer;
    [SerializeField] private bool isBroken = false;

    public bool IsBroken
    {
        get
        {
            return isBroken;
        }
    }

    public Vector3 PosForFixing
    {
        get
        {
            return placeForFixing.position;
        }
    }

    public void BrokeIt()
    {
        isBroken = true;
    }

    public void FixIt(Instrument instrument, Action callback)
    {
        if (targetInstrumentType != instrument)
            return;

        timer?.Dispose();
        timer = Observable.Timer(TimeSpan.FromSeconds(fixingTime))
            .TakeUntilDisable(gameObject)
            .Subscribe( _ => 
            {
                isBroken = false;
                callback?.Invoke();
                EventManager.ProblemFixed();
            });
    }

}
