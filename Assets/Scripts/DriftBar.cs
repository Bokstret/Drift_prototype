using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DriftBar : MonoBehaviour
{
    public static int status;
    public Transform progressBar;
    public Transform percentBar;
    public Transform center;
    public float currentAmount;
    public Transform stop;
    [SerializeField]
    private float speed;

    void Update()
    {
        if (GoToClick.drifting == true)
            ShowUI();

        else
            ResetUI();

        if (status == 1)
            progressBar.GetComponent<Image>().fillClockwise = true;
        else
            progressBar.GetComponent<Image>().fillClockwise = false;

        if (currentAmount < 100)
        {
            
            currentAmount += speed * Time.deltaTime;
            percentBar.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
        }
        else
        {
            stop.GetComponent<Text>().enabled = true;
            if (GoToClick.drifting == false)
                Invoke("ResetUI", 0.5f);

        }
        progressBar.GetComponent<Image>().fillAmount = currentAmount / 100;

    }

    void ResetUI()
    {
        currentAmount = 0;
        percentBar.GetComponent<Text>().text = "0%";
        percentBar.GetComponent<Text>().enabled = false;
        stop.GetComponent<Text>().enabled = false;
        progressBar.GetComponent<Image>().enabled = false;
        center.GetComponent<Image>().enabled = false;
    }
    void ShowUI()
    {
        percentBar.GetComponent<Text>().enabled = true;
        progressBar.GetComponent<Image>().enabled = true;
        center.GetComponent<Image>().enabled = true;
    }
}
