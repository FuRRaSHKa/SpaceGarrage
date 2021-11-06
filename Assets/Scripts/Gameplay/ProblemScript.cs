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
    [SerializeField] private TimerSlider timer;

    private float fixingTime;
    private float putOutFireTime;
    private float timeUntilItBurn;
    private float timeToLoseAfterFire;

    private SpriteRenderer spriteRenderer;

    private IDisposable timerToFix;
    private bool isBroken = false;
    private bool isBurned = false;
    private bool enableToSpawn = true;

    public bool IsBroken
    {
        get
        {
            return isBroken;
        }
    }

    public bool EnableToSpawn
    {
        get
        {
            return enableToSpawn;
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
        ToFire();

        isBroken = true;
        enableToSpawn = false;
        normal.enabled = false;
        spriteRenderer.enabled = true;
    }

    public void ToFire()
    {
        timer.PlayTimer(timeUntilItBurn, () =>
        {
            if (!isBroken)
                return;

            burnEffect.enabled = true;
            isBurned = true;
            AfterFireTimer();
        });
    }

    public bool FixIt(Instrument instrument, Action callback)
    {

        if (isBurned)
            return PutOutFire(instrument, callback);

        if (targetInstrumentType != instrument)
            return false;

        timer.StopTimer();
        timerToFix?.Dispose();
        timerToFix = Observable.Timer(TimeSpan.FromSeconds(fixingTime))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ =>
           {
               isBroken = false;
               normal.enabled = true;
               spriteRenderer.enabled = false;
               callback?.Invoke();
               EventManager.ProblemFixed();
               MakeEnableToSpawn();
           });

        return true;
    }

    private void MakeEnableToSpawn()
    {
        Observable.Timer(TimeSpan.FromSeconds(3))
             .TakeUntilDisable(gameObject)
             .Subscribe(_ =>
             {
                 enableToSpawn = true;
             });
    }

    private bool PutOutFire(Instrument instrument, Action callback)
    {
        if (Instrument.Extinguisher != instrument)
            return false;

        timer.StopTimer();
        timerToFix?.Dispose();
        timerToFix = Observable.Timer(TimeSpan.FromSeconds(putOutFireTime))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ =>
            {
                isBurned = false;
                burnEffect.enabled = false;
                ToFire();
                callback?.Invoke();
            });

        return true;

    }

    private void OnRoundEnd(bool isWin)
    {
        timerToFix?.Dispose();
        timer.StopTimer();

        isBroken = false;
        isBurned = false;
        enableToSpawn = true;

        if (isWin)
        {
            spriteRenderer.enabled = false;
            normal.enabled = true;
            burnEffect.enabled = false;
        }
    }

    private void AfterFireTimer()
    {
        timer.PlayTimer(timeToLoseAfterFire, () =>
        {
            if (!isBurned)
                return;

            EventManager.EndRound(false);
        });
    }

}
