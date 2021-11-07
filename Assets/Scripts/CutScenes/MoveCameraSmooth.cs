using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraSmooth : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 target;
    private float scale;
    private Camera cam;

    private bool isMoving = false;
    private float t;

    private void Start()
    {
        cam = Camera.main;
    }

    public void MoveTo(Vector3 target, float scale)
    {
        isMoving = true;
        t = 0;

        this.scale = scale;
        this.target = target;
    }

    private void Update()
    {
        if (!isMoving)
            return;


        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, scale, speed * Time.deltaTime);
        t = Mathf.Lerp(t, 1, speed * Time.deltaTime);

        if (t > .95f)
        {
            isMoving = false;
        }

    }

}
