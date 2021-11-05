using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    private Transform target;

    private void LateUpdate()
    {
        if(target != null)
            transform.position = target.position;
    }

    public void SetTarger(Transform target)
    {
        this.target = target;
    }

}
