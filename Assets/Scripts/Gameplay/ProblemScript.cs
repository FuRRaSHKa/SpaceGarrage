using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class ProblemScript : MonoBehaviour
{
    [SerializeField] private Instrument targetInstrumentType;
    [SerializeField] private Transform placeForFixing;
    [SerializeField] private SpriteRenderer burnEffect;
    [SerializeField] private SpriteRenderer normal;

    private float fixingTime;
    private float putOutFireTime;
    private float timeUntilItBurn;
    private float timeToLoseAfterFire;

    private SpriteRenderer spriteRenderer;
 
    private IDisposable timerToFix;
    private IDisposable timerToBurn;
    private IDisposable loseTimer;
    private bool isBroken = false;
    private bool isBurned = false;

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
        EventManager.onRoundEnd += OnRoundEnd;
    }

    public void Init(RoundData roundData)
    {
        this.fixingTime = roundData.timeToFix;
        this.putOutFireTime = roundData.timeToPutOutFire;
        this.timeUntilItBurn = roundData.timeUntilFire;
        this.timeToLoseAfterFire = roundData.timeToLoseAfterFire;
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
                AfterFireTimer();
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

        loseTimer?.Dispose();
        timerToFix?.Dispose();
        timerToFix = Observable.Timer(TimeSpan.FromSeconds(putOutFireTime))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ =>
            {
                isBurned = false;
                burnEffect.enabled = false;
                callback?.Invoke();
            });

        return true;

    }

    private void OnRoundEnd(bool isWin)
    {
        timerToFix?.Dispose();
        timerToBurn?.Dispose();
        loseTimer?.Dispose();

        isBroken = false;
        isBurned = false;

        if (isWin)
        {
            spriteRenderer.enabled = false;
            normal.enabled = true;
            burnEffect.enabled = false;
        }
    }

    private void AfterFireTimer()
    {
        loseTimer?.Dispose();
        loseTimer = Observable.Timer(TimeSpan.FromSeconds(timeToLoseAfterFire))
            .TakeUntilDisable(gameObject)
            .Subscribe( _ => 
            {
                if (!isBurned)
                    return;

                EventManager.EndRound(false);
            });
    }

}
