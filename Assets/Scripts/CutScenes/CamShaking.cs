using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class CamShaking : MonoBehaviour
{
    private IDisposable disposable;

    public void Shaking(float magnitude, float duration)
    {
        float elapsed = 0;
        Vector3 startPos = transform.position;

        disposable?.Dispose();
        disposable = Observable.EveryUpdate()
            .TakeUntilDisable(gameObject)
            .Where(w =>
            {
                Vector3 pos = startPos;

                pos.x += UnityEngine.Random.Range(-1, 1) * magnitude;
                pos.y += UnityEngine.Random.Range(-1, 1) * magnitude;
                transform.position = pos;

                elapsed += Time.deltaTime;
                if (elapsed > duration)
                    return true;

                return false;
            })
            .Subscribe(_ =>
           {
               disposable?.Dispose();
               transform.position = startPos;
            });

    }
}
