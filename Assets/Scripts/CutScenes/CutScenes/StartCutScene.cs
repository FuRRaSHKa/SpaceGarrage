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

    private Camera cam;
    private MoveCameraSmooth cameraSmooth;
    private Vector3 starCamPos;
    private float startCamSize;

    private bool isStart = true;

    private void Start()
    {
        
        Timer(() =>
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/scene1_boss_call");

            cam = Camera.main;
            startCamSize = cam.orthographicSize;
            starCamPos = cam.transform.position;
            cameraSmooth = cam.GetComponent<MoveCameraSmooth>();
            cameraSmooth.MoveTo(placeForCam.position, sizeCam);
            Timer(() =>
            {
                prorab.transform.parent.gameObject.SetActive(true);

                StartScene();
            });
        });
    }

    private void Update()
    {
        if (isStart)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            prorab.transform.parent.gameObject.SetActive(false);
            cameraSmooth.MoveTo(starCamPos, startCamSize);
            Timer(() =>
            {
                
                
                EventManager.StartRound();
            });
            Debug.Log(true);
            isStart = true;
        }
    }

    public void StartScene()
    {
        isStart = false;
        NextLine(0, 5, true, NextPhase);
    }

    private void NextLine(int count, int maxCount, bool isBoss, Action callback)
    {
        if (isStart)
        {
            return;
        }

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
