using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class UITimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

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
        text.enabled = true;
        this.callback = callback;
    }

    public void StopTimer()
    {
        isStarded = false;
        text.enabled = false;
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

        text.text = Mathf.FloorToInt(targetTime - curTime).ToString();

    }
}
