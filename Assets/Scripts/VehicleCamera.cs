using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class VehicleCamera : MonoBehaviour
{
    public Transform target;
    public float smooth = 0.3f;
    public float distance = 5.0f;
    public float height = 1.0f;
    public float Angle = 20;
    public LayerMask lineOfSightMask = 0;
    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;
    [HideInInspector]
    public int Switch;
    private float restTime = 0.0f;

    void Update()
    {
        if (!target) 
            return;

        if (restTime != 0.0f)
            restTime = Mathf.MoveTowards(restTime, 0.0f, Time.deltaTime);

       float xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x,
       target.eulerAngles.x + Angle, ref xVelocity, smooth);

        float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
        target.eulerAngles.y, ref yVelocity, smooth);

        transform.eulerAngles = new Vector3(xAngle, yAngle, 0.0f);

        var direction = transform.rotation * -Vector3.forward;
        var targetDistance = AdjustLineOfSight(target.position + new Vector3(0, height, 0), direction);

        transform.position = target.position + new Vector3(0, height, 0) + direction * targetDistance;

        float AdjustLineOfSight(Vector3 target, Vector3 direction)
        {
            RaycastHit hit;

            if (Physics.Raycast(target, direction, out hit, distance, lineOfSightMask.value))
                return hit.distance;
            else
                return distance;
        }
    }
}