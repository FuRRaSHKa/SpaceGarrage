using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class StartCutScene : MonoBehaviour
{
    [SerializeField] private TextingScript prorab;
    [SerializeField] private TextingScript bossPhone;
    [SerializeField] private Transform placeForCam;
    [SerializeField] private float sizeCam;
    [SerializeField] private string pathSound;

    private Camera cam;
    private MoveCameraSmooth cameraSmooth;
    private Vector3 starCamPos;
    private float startCamSize;

    private void Start()
    {
        cam = Camera.main;
        startCamSize = cam.orthographicSize;
        starCamPos = cam.transform.position;
        cameraSmooth = cam.GetComponent<MoveCameraSmooth>();
        cameraSmooth.MoveTo(placeForCam.position, sizeCam);
        Timer(() => 
        {
            FMODUnity.RuntimeManager.PlayOneShot(pathSound);
            prorab.transform.parent.gameObject.SetActive(true);
            
            StartScene();
        });  
    }

    public void StartScene()
    {
        NextLine(0, 5, true, NextPhase);
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

    private void NextPhase()
    {
        cam.GetComponent<CamShaking>().Shaking(.1f, 1);
        Timer(() =>
        {
            NextLine(0, 3, true, () =>
            {
                Timer(() =>
                {
                    prorab.transform.parent.gameObject.SetActive(false);
                    cameraSmooth.MoveTo(starCamPos, startCamSize);
                    EventManager.StartRound();
                });
            });
        }, 2);

    }

    private void Timer(Action callback, float time = 2f)
    {
        Observable.Timer(TimeSpan.FromSeconds(time))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ => callback?.Invoke());
    }

}
