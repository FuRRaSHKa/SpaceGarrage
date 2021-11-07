using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class FourthCutScene : MonoBehaviour
{
    [SerializeField] private MansMovement mech;
    [SerializeField] private MovingCharacter pop;
    [SerializeField] private MovingCharacter boss;
    [SerializeField] private TextingScript prorab;
    [SerializeField] private TextingScript mechText;
    [SerializeField] private Transform placeForCam;
    [SerializeField] private float sizeCam;
    [SerializeField] private Transform[] waypointsBatyoshka;
    [SerializeField] private Transform[] waypointsBoss;
    [SerializeField] private Animator shipAnim;
    [SerializeField] private CamShaking shipShaking;
    [SerializeField] private CutSceneManager cutSceneManager;

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

    public void StartScene()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/scene5_win");

        NextWayPoint(pop, 0, null, waypointsBatyoshka);
        NextWayPoint(boss, 0, FlyUp, waypointsBoss);
    }

    private void NextWayPoint(MovingCharacter move, int i, Action callback, Transform[] waypoints)
    {
        if (i >= waypoints.Length)
        {
            callback?.Invoke();
            return;
        }

        move.MoveTo(waypoints[i].position, 5, () => NextWayPoint(move, ++i, callback, waypoints));
    }

    private void FlyUp()
    {
        pop.gameObject.SetActive(false);
        boss.gameObject.SetActive(false);
        Timer(() =>
        {
            shipShaking.Shaking(.1f, 1);

            Timer(() =>
            {
                shipShaking.Shaking(.1f, 1);
                Timer(() =>
                {
                    shipAnim.enabled = true;
                    Timer(() => Timer(Texting, 3));
                }, 2f);

            }, 2f);
        });
        
    }

    private void Texting()
    {
        mech.MoveTo(prorab.transform.position + Vector3.one * -1, () =>
        {
            cameraSmooth.MoveTo(placeForCam.position , sizeCam);
            Timer(() =>
            {
                prorab.transform.parent.gameObject.SetActive(true);
                NextLine(0, 6, false, () => Timer(() =>
                {
                    prorab.TextLine(() => Timer(() =>
                    {

                        cutSceneManager.StartWin();

                    }));
                    
                }));
            });
        });
    }


    private void NextLine(int count, int maxCount, bool isMech, Action callback)
    {
        if (count >= maxCount)
        {
            Timer(() => callback?.Invoke(), 1);

            return;
        }

        if (isMech)
        {
            mechText.TextLine(() => Timer(() =>
            {
                mechText.ClearText();
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

    private void Timer(Action callback, float time = 2f)
    {
        Observable.Timer(TimeSpan.FromSeconds(time))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ => callback?.Invoke());
    }
}
