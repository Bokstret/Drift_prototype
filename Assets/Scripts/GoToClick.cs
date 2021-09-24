using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GoToClick : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 lookAtTarget;
    private Vector3 mousePosition;
    private Quaternion playerRot;
    private int screenCenter;
    private int side;
    private int points;
    private int status = 1;
    private int driftTime;
    private float rotationSpeed = 5;
    private float moveSpeed = 5;
    private bool earning = true;
    public static bool moving = false;
    public static bool drifting = false;
    public Transform pointsBar;
    public GameObject prefab;

    void Start()
    {
        screenCenter = Screen.width / 2;
        InvokeRepeating("GetPoints", 0, 1); 
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!earning)
            {
                earning = true;
                InvokeRepeating("GetPoints", 0, 1);
            }
            CarController.horizontalInput = 0;
            mousePosition = Input.mousePosition;
            driftTime = 0;
            CancelInvoke("LosePoints");
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
        pointsBar.GetComponent<Text>().text = "Очков: " + (points).ToString();
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
            drifting = false;
            moving = true;
            status = 1;
            Instantiate(prefab, hit.point, Quaternion.identity);

            if (mousePosition.x < screenCenter)
            {
                side = -1;
                DriftBar.status = -1;
            }

            if (mousePosition.x > screenCenter)
            {
                side = 1;
                DriftBar.status = 1;
            }



        }
    }

    void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, rotationSpeed * Time.deltaTime);
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

    void LosePoints()
    {
        driftTime += 1;

        if (driftTime >= 8)
        {
            CancelInvoke("GetPoints");
            earning = false;
            points -= 21;
        }
    }


    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Dangerous")
        {
            GoToClick.drifting = false;
            CarController.horizontalInput = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Detect")
        {
            moving = false;
            InvokeRepeating("LosePoints", 0, 1);
            drifting = true;
            status = 2;
            Destroy(other.gameObject);
        }
    }
}