using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class MovingCharacter : MonoBehaviour
{
    [SerializeField] private Animator animator;

    Vector2 startPos;
    Vector2 targetPos;
    float journeyTime;
    float time = 0;

    [SerializeField] bool isMoving = false;
    Action callback = null;

    public void MoveTo(Vector2 pos, float speed, Action callback)
    {
        startPos = transform.position;
        targetPos = pos;
        
        journeyTime = (startPos - targetPos).magnitude / speed;
        time = 0;
        isMoving = true;
        this.callback = callback;
    }

    public void Update()
    {
        if (!isMoving)
            return;

       
        time += Time.deltaTime;
        transform.position = Vector2.Lerp(startPos, targetPos, time / journeyTime);

        if (journeyTime <= time)
        {
            isMoving = false;
            callback?.Invoke();
        }
    }

}
