using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_AudioVolume : MonoBehaviour
{

    Text display;

    // Use this for initialization
    void Start()
    {
        display = GetComponent<Text>();
        update_display(AudioListener.volume);
    }

    // Update is called once per frame
    void Update()
    {
        const float volume_change = 0.1f;
        if (Input.GetKeyDown(KeyCode.PageUp) ||
            (Input.GetAxis("DPAD_VERT") > 0.5)) // DPAD up
        {
            change_volume(+volume_change);

        }
        if (Input.GetKeyDown(KeyCode.PageDown) ||
            (Input.GetAxis("DPAD_VERT") < -0.5)) // DPAD down
        {
            change_volume(-volume_change);
        }

    }

    private void change_volume(float change_value)
    {
        AudioListener.volume += change_value;
        update_display(AudioListener.volume);
    }

    private void update_display(float new_volume)
    {
        const string display_format = "Audio Volume: {0}%";

        int volume_percent = (int)(100 * new_volume);

        var new_text = string.Format(display_format, volume_percent);

        display.text = new_text;
    }
}
