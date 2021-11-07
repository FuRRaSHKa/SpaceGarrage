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
    int id = 0;

    public void TextLine(Action callback)
    {
        if (id >= texts.Length)
            return;

        int i = 0;
        disposable?.Dispose();
        disposable = Observable.Interval(TimeSpan.FromSeconds(.05f))
            .TakeUntilDisable(gameObject)
            .Where(w =>
           {
               textMeshPro.text += texts[id][i];
               i++;
               if (i >= texts[id].Length)
               {
                   return true;
               }

               return false;
           })
            .Subscribe(_ =>
           {
               id++;
               disposable?.Dispose();
               callback?.Invoke();
           });
    }

    public void ClearText()
    {
        textMeshPro.text = "";
    }
}
