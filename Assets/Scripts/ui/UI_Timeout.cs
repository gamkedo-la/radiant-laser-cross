using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Timeout : MonoBehaviour
{
    public Text time_text; // TODO: replace by graphics
    public Image bar;
    private RectTransform bar_rect;
    private float bar_max_height = 0f;
    private float bar_initial_y = 0f;

    public float MaxTime { get; set; }


    // Use this for initialization
    void Awake()
    {
        if (!time_text)
        {
            Debug.LogError("Missing TimeoutSystem.display");
        }
        else
        {
            hide();
            bar_rect = bar.GetComponent<RectTransform>();
            bar_max_height = bar_rect.sizeDelta.y;
            bar_initial_y = bar_rect.anchoredPosition.y;
        }
    }

    public void show(float seconds)
    {
        update_display(seconds);
        time_text.enabled = true;
        bar.enabled = true;
    }

    public void hide()
    {
        time_text.enabled = false;
        bar.enabled = false;
    }

    private void update_display(float seconds_left)
    {
        const string test_format = "{0:0.00}";
        var new_text = string.Format(test_format, seconds_left);
        // TODO: if seconds_left is low, make it red or something
        
        time_text.text = new_text;
        update_fore_bar(seconds_left);
    }

    private void update_fore_bar(float seconds_left)
    {
        var bar_height = bar_max_height * (seconds_left / MaxTime);
        bar_rect.sizeDelta = new Vector2(bar_rect.sizeDelta.x, bar_height);
        bar_rect.anchoredPosition = new Vector2(bar_rect.anchoredPosition.x, bar_initial_y - (bar_max_height - bar_height)/2f);
    }
}
