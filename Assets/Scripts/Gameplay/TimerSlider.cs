using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerSlider : MonoBehaviour
{

    [SerializeField] private SpriteRenderer slider;

    private float curTime;
    private float targetTime;
    private bool isStarded = false;
    private float startSize;
    private Action callback;

    private void Start()
    {    
        startSize = transform.localScale.x;
    }

    public void PlayTimer(float targetTime, Action callback = null)
    {
        isStarded = true;
        curTime = 0;
        this.targetTime = targetTime;
        slider.enabled = true;
        this.callback = callback;
    }

    public void StopTimer()
    {
        isStarded = false;
        slider.enabled = false;
    }

    private void Update()
    {
        if (!isStarded)
            return;

        curTime += Time.deltaTime;
        if (curTime >= targetTime)
        {
            curTime = targetTime;
            isStarded = false;
            callback?.Invoke();
        }

        Vector3 localScale = transform.localScale;
        localScale.x =  (1 - curTime /targetTime)  * startSize;
        localScale.x = Mathf.Clamp(localScale.x, 0, startSize);
        transform.localScale = localScale;

    }

}
