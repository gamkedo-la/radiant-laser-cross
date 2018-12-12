using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioSource menu_change_sound;
    public AudioSource menu_validate_sound;
    public AudioSource menu_cancel_sound;


    public List<Button> all_buttons;


    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            StartCoroutine(run_exit());
        }
    }

    public void start_new_game()
    {
        StartCoroutine(load_new_game()); ;
    }


    public void display_credits()
    {
        StartCoroutine(load_credits());
    }

    private void deactivate_all_buttons()
    {
        foreach (var button in all_buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    private const float seconds_to_next_screen = 1.0f;

    private IEnumerator run_exit()
    {
        deactivate_all_buttons();
        menu_cancel_sound.Play();
        yield return new WaitForSeconds(seconds_to_next_screen);
        Application.Quit(); // FIXME: might cause problems in web builds
    }

    private IEnumerator load_credits()
    {
        deactivate_all_buttons();
        menu_validate_sound.Play();
        yield return new WaitForSeconds(seconds_to_next_screen);

        // SceneManager.LoadScene("Credits", LoadSceneMode.Single); // TODO: UNCOMMENT THIS TO LOAD CREDITS WHEN THE BUTTON IS PRESSED!
    }

    private IEnumerator load_new_game()
    {
        deactivate_all_buttons();
        menu_validate_sound.Play();
        yield return new WaitForSeconds(seconds_to_next_screen);

        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

}
