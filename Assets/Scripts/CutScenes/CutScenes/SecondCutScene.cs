using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class SecondCutScene : MonoBehaviour
{
    [SerializeField] private MovingCharacter bossMoving;
    [SerializeField] private TextingScript prorab;
    [SerializeField] private TextingScript bossPhone;
    [SerializeField] private Transform placeForCam;
    [SerializeField] private float sizeCam;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private string pathSound;
    private Camera cam;
    private Vector3 starCamPos;
    private float startCamSize;
    private MoveCameraSmooth cameraSmooth;

    private void Start()
    {
        cam = Camera.main;
        startCamSize = cam.orthographicSize;
        starCamPos = cam.transform.position;
        cameraSmooth = cam.GetComponent<MoveCameraSmooth>();
    }

    public void StarScene()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/scene3_boss_came");
        Timer(() =>
        {
            NextWayPoint(0, Texting);
        });    
    }

    private void NextWayPoint(int i, Action callback)
    {
        if (i >= waypoints.Length)
        {
            callback?.Invoke();
            return;
        }

        bossMoving.MoveTo(waypoints[i].position, 5,() => NextWayPoint(++i, callback));
    }

    private void NextWayPointInverse(int i, Action calcback)
    {
        if (i < 0)
        {
            calcback?.Invoke();
            return;
        }

        bossMoving.MoveTo(waypoints[i].position, 5, () => NextWayPointInverse(--i, calcback));
    }

    private void Texting()
    {
        cameraSmooth.MoveTo(placeForCam.position, sizeCam);
        prorab.transform.parent.gameObject.SetActive(true);
        NextLine(0, 3, true, GoOut);
    }

    private void NextLine(int count, int maxCount, bool isBoss, Action callback)
    {
        if (count >= maxCount)
        {
            Timer(() => callback?.Invoke(), 1);

            return;
        }

        if (isBoss)
        {
            bossPhone.TextLine(() => Timer(() =>
            {
                bossPhone.ClearText();
                NextLine(++count, maxCount, false, callback);
            }));
        }
        else
        {
            prorab.TextLine(() => Timer(() =>
            {
                prorab.ClearText();
                NextLine(++count, maxCount, true, callback);
            }));
        }
    }

    private void GoOut()
    {
        prorab.transform.parent.gameObject.SetActive(false);
        cameraSmooth.MoveTo(starCamPos, startCamSize);
        NextWayPointInverse(waypoints.Length - 1, () => 
        {
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
