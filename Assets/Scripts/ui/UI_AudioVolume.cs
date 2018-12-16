using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_AudioVolume : MonoBehaviour
{

    private Text display;
    private float last_volume_change;
    const float wait_time_before_next_volume_change = 1.0f / 8.0f;

    // Use this for initialization
    void Start()
    {
        display = GetComponent<Text>();
        update_display(AudioListener.volume);
        last_volume_change = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        const float volume_change = 0.1f;
        if (Input.GetKeyDown(KeyCode.PageUp)
        ||  Input.GetAxis("DPAD_VERT") == 1) // DPAD up
        {
            change_volume(+volume_change);

        }
        if (Input.GetKeyDown(KeyCode.PageDown)
        ||  Input.GetAxis("DPAD_VERT") == -1) // DPAD down
        {
            change_volume(-volume_change);
        }

    }

    private void change_volume(float change_value)
    {
        var now = Time.time;
        var time_since_last_volume_change = now - last_volume_change;
        if (time_since_last_volume_change >= wait_time_before_next_volume_change)
        {
            last_volume_change = now;
            var new_volume = AudioListener.volume + change_value;
            new_volume = Mathf.Clamp(new_volume, 0.0f, 1.0f);
            AudioListener.volume = new_volume;
            update_display(AudioListener.volume);
        }
    }

    private void update_display(float new_volume)
    {
        const string display_format = "Audio Volume: {0}%";

        int volume_percent = (int)(100 * new_volume);

        var new_text = string.Format(display_format, volume_percent);

        display.text = new_text;
    }
}
