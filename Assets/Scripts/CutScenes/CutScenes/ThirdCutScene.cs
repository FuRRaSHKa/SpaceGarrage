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

    public void StarScene()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pathSound);
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
        if (i >= waypoints.Length)
        {
            callback?.Invoke();
            return;
        }

        popMoving.MoveTo(waypoints[i].position, 5, () => NextWayPoint(++i, callback));
    }

    private void NextWayPointInverse(int i, Action calcback)
    {
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
