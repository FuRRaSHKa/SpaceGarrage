using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class JoraController : MonoBehaviour
{
    [SerializeField] private Sprite spriteStandart;
    [SerializeField] private Sprite spriteChoose;
    [SerializeField] private Sprite spriteCommand;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private IDisposable disposable;

    public void MakeCommand(Vector2 pos)
    {
        float angle = Vector2.SignedAngle(Vector2.right, pos - (Vector2)transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        spriteRenderer.sprite = spriteCommand;

        disposable?.Dispose();
        disposable = Observable.Timer(TimeSpan.FromSeconds(.5f))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ =>
           {
               spriteRenderer.sprite = spriteStandart;
           });
    }

    public void ChooseMech( Vector2 pos)
    {
        float angle = Vector2.SignedAngle(Vector2.right, pos - (Vector2)transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        spriteRenderer.sprite = spriteChoose;

        disposable?.Dispose();
        disposable = Observable.Timer(TimeSpan.FromSeconds(.5f))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ =>
            {
                spriteRenderer.sprite = spriteStandart;
            });
    }

}
