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
    [SerializeField] private float PutOutFireTime;
    [SerializeField] private float timeUntilItBurn;
    [SerializeField] private SpriteRenderer burnEffect;
    [SerializeField] private SpriteRenderer normal;

    private SpriteRenderer spriteRenderer;
 
    private IDisposable timerToFix;
    private IDisposable timerToBurn;
    [SerializeField] private bool isBroken = false;
    [SerializeField] private bool isBurned = false;

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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void BrokeIt()
    {
        timerToBurn?.Dispose();
        timerToBurn = Observable.Timer(TimeSpan.FromSeconds(timeUntilItBurn))
            .TakeUntilDisable(gameObject)
            .Subscribe( _ => 
            {
                if (!isBroken)
                    return;

                burnEffect.enabled = true;
                isBurned = true;
            });

        isBroken = true;
        normal.enabled = false;
        spriteRenderer.enabled = true;
    }

    public bool FixIt(Instrument instrument, Action callback)
    {
        
        if (isBurned)
            return PutOutFire(instrument, callback);

        if (targetInstrumentType != instrument)
            return false;

        timerToBurn?.Dispose();
        Debug.Log(instrument);
        timerToFix?.Dispose();
        timerToFix = Observable.Timer(TimeSpan.FromSeconds(fixingTime))
            .TakeUntilDisable(gameObject)
            .Subscribe( _ => 
            {
                isBroken = false;
                normal.enabled = true;
                spriteRenderer.enabled = false;
                callback?.Invoke();
                EventManager.ProblemFixed();
            });

        return true;
    }

    private bool PutOutFire(Instrument instrument, Action callback)
    {
        if (Instrument.Extinguisher != instrument)
            return false;

        timerToFix?.Dispose();
        timerToFix = Observable.Timer(TimeSpan.FromSeconds(PutOutFireTime))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ =>
            {
                isBurned = false;
                burnEffect.enabled = false;
                callback?.Invoke();
            });

        return true;

    }

}
