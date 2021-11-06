using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using TMPro;

public class TextingScript : MonoBehaviour
{
    [SerializeField] private string[] texts;
    [SerializeField] private TextMeshPro textMeshPro;

    private IDisposable disposable;

    public void TextLine(int id, Action callback)
    {
        int i = 0;
        disposable?.Dispose();
        disposable = Observable.EveryUpdate()
            .TakeUntilDisable(gameObject)
            .Where( w => 
            {
                if (i >= texts[id].Length)
                {
                    return true;
                }

                textMeshPro.text += texts[i];
                i++;
                return false;
            })
            .Subscribe( _ => 
            {
                disposable?.Dispose();
                callback?.Invoke();
            });
    }

}
