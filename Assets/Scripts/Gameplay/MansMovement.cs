using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class MansMovement : MonoBehaviour
{

    [SerializeField] private float speed = 100f;
    [SerializeField] private float nextWaypointDistance = 1f;

    private Path path = null;
    private int currentWaypoint;
    private Seeker seeker;

    private Action callback;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
    }

    public void MoveTo(Vector2 target, Action callback = null)
    {
        this.callback = callback;
        seeker.StartPath(transform.position, target, OnPathComplete);
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            callback?.Invoke();
            path = null;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, path.vectorPath[currentWaypoint], speed * Time.deltaTime);
        float angle = Vector2.SignedAngle(Vector2.right, (Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), 10 * Time.deltaTime);

        if ((transform.position - path.vectorPath[currentWaypoint]).magnitude < nextWaypointDistance)
            currentWaypoint++;
    }

}
