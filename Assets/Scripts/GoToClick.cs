using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine.UI;
using UnityEngine;

public class GoToClick : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 lookAtTarget;
    private Quaternion playerRot;
    private int side;
    private int points;
    private int status = 1;
    private int driftTime;
    private float rotationSpeed = 5;
    private float moveSpeed = 10;
    public static bool moving = false;
    public static bool drifting = false;
    public Transform pointsBar;
    public Transform deathStatus;

    void Start()
    {
        InvokeRepeating("GetPoints", 0, 1);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            driftTime = 0;
            CancelInvoke("losePoints");
            SetCarPosition();
        }
        if (moving)
        {
            Move();
        }
            
        else
        {
            if (drifting)
            {
                Drift(side);
            }
        }
        pointsBar.GetComponent<Text>().text = (points).ToString() + " points";
    }

    void SetCarPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            targetPosition = hit.point;

            lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y, targetPosition.z - transform.position.z);
            playerRot = Quaternion.LookRotation(lookAtTarget);

            if (Mathf.Abs(targetPosition.y - transform.position.y) < 0.1f)
                if (targetPosition.z > transform.position.z)
                {
                    drifting = false;
                    moving = true;
                    status = 1;
                }


            if (targetPosition.x < transform.position.x)
            {
                side = -1;
                DriftBar.status = -1;
            }

            else
            {
                side = 1;
                DriftBar.status = 1;
            }
        }
    }
    void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, rotationSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (transform.position == targetPosition)
        {
            moving = false;
            InvokeRepeating("losePoints", 0, 1);
            drifting = true;
            status = 2;
        }
            
    }

    void Drift(int side)
    {
        CarController.horizontalInput = side;
    }

    void GetPoints()
    {
        if (status == 1)
            points += 1;
        else
            points += 3;
    }

    void losePoints()
    {
        driftTime += 1;

        if (driftTime >= 8)
            points -= 21;   
    }

    void TurnOffDeath()
    {
        deathStatus.GetComponent<Text>().enabled = false;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Dangerous")
        {
            deathStatus.GetComponent<Text>().enabled = true;
            Invoke("TurnOffDeath", 1.5f);
        }
    }
}