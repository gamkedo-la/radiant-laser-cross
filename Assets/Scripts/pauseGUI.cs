﻿using System.Collections;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pause Toggled!");
            if (!pausePanel.activeInHierarchy)
            {
                PauseGame();
            }
            else if (pausePanel.activeInHierarchy)
            {
                ContinueGame();
            }
        }
    }
    private void PauseGame()
    {
        Debug.Log("PAUSING GAME!");
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }
    private void ContinueGame()
    {
        Debug.Log("UNPAUSING GAME!");
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //enable the scripts again
    }
}