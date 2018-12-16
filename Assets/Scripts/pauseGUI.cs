using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pauseGUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
    }

    // Returns true if we are paused, false if we are not paused.
    public bool toggle_pause()
    {
        Debug.Log("Pause Toggled!");
        if (!pausePanel.activeInHierarchy)
        {
            PauseGame();
            return true;
        }
        else
        {
            ResumeGame();
            return false;
        }
    }

    private void PauseGame()
    {
        Debug.Log("PAUSING GAME!");
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }

    private void ResumeGame()
    {
        Debug.Log("UNPAUSING GAME!");
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //enable the scripts again
    }
}