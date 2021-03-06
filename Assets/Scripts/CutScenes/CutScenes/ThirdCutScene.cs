using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class ThirdCutScene : MonoBehaviour
{
    [SerializeField] private MovingCharacter popMoving;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private string pathSound;

    private FMOD.Studio.EventInstance instance;

    private bool isStart = true;

    public void StarScene()
    {
        isStart = false;

        instance = FMODUnity.RuntimeManager.CreateInstance("event:/scene4_pop");
        instance.start();

        Timer(() =>
        {
            NextWayPoint(0, ()=>
            {
                Timer(() => GoOut(), 3);
            });
        });
    }

    private void NextWayPoint(int i, Action callback)
    {
        if (isStart)
            return;

        if (i >= waypoints.Length)
        {
            callback?.Invoke();
            return;
        }

        popMoving.MoveTo(waypoints[i].position, 5, () => NextWayPoint(++i, callback));
    }

    private void NextWayPointInverse(int i, Action calcback)
    {
        if (isStart)
            return;

        if (i < 0)
        {
            calcback?.Invoke();
            return;
        }

        popMoving.MoveTo(waypoints[i].position, 5, () => NextWayPointInverse(--i, calcback));
    }

    private void GoOut()
    {
        NextWayPointInverse(waypoints.Length - 1, () =>
        {
            Debug.Log(true);
            Timer(() =>
            {
                
                EventManager.StartRound();
            });
        });
    }

    private void Timer(Action callback, float time = 2f)
    {
        Observable.Timer(TimeSpan.FromSeconds(time))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ => callback?.Invoke());
    }

}
