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

    bool isMoving = false;
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
        float angle = Vector2.SignedAngle(Vector2.right, (Vector2)targetPos - (Vector2)transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), 10 * Time.deltaTime);

        if (journeyTime <= time)
        {
            isMoving = false;
            callback?.Invoke();
        }
    }

}
