using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Timeout : MonoBehaviour
{
    public Text display; // TODO: replace by graphics


    // Use this for initialization
    void Start()
    {
        if (!display)
        {
            Debug.LogError("Missing TimeoutSystem.display");
        }
        else
        {
            display.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void show(float seconds)
    {
        update_display(seconds);
        display.enabled = true;
    }

    public void hide()
    {
        display.enabled = false;
    }

    private void update_display(float seconds_left)
    {
        const string test_format = "TIMEOUT {0:0.00}";

        var new_text = string.Format(test_format, seconds_left);
        // TODO: if seconds_left is low, make it red or something


        display.text = new_text;
    }


}
