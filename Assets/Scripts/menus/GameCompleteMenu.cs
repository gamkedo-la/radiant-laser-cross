using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompleteMenu : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }


    public void on_title_screen_button()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

}
