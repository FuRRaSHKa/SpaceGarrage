using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void Start()
    {
        Debug.Log(true);
    }

    public void StartCutScene()
    {
        Observable.Timer(TimeSpan.FromSeconds(1f))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ =>
           {
               anim.SetTrigger("Next");
           });
    }

    public void StartWin()
    {
        anim.SetTrigger("Win");
    }

    public void StartLose()
    {
        anim.SetTrigger("Lose");
    }

    public void EndCutScene()
    {
        EventManager.StartRound();
    }

    public void EndLose()
    {
        Observable.Timer(TimeSpan.FromSeconds(1))
            .TakeUntilDisable(gameObject)
            .Subscribe( _ => 
            {
                SceneManager.LoadScene(0);
            });
    }
}
