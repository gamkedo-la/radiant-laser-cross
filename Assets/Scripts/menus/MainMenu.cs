using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit(); // FIXME: might cause problems in web builds
    }

    public void start_new_game()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }


    public void display_credits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }


}
