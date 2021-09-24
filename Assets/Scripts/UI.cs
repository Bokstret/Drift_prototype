using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour

{
    public Transform guide;
    public Transform restart;
    private GoToClick script;

    private void Start()
    {
        restart.gameObject.SetActive(false);
        Time.timeScale = 0;
        script = GameObject.Find("Car").GetComponent<GoToClick>();
        script.enabled = false;
    }
    public void HideUI()
    {
        Time.timeScale = 1;
        guide.gameObject.SetActive(false);
        restart.gameObject.SetActive(true);
        script.enabled = true;
    }

    public void Restart()
    {
        GoToClick.drifting = false;
        CarController.horizontalInput = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
