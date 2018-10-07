using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Overload : MonoBehaviour {

    public rlc.OverloadSystem overload_system;

    public Text text_to_replace_by_some_hud_bar;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (!overload_system)
        {
            overload_system = acquire_overload_system();
            if (overload_system == null)
            {
                return;
            }
        }

        update_display();
    }

    private rlc.OverloadSystem acquire_overload_system()
    {
        var player = GameObject.Find("laser_cross");
        if (player)
        {
            return player.GetComponent<rlc.OverloadSystem>();
        }

        return null;
    }

    private void update_display()
    {
        // TODO: replace this by something graphic (a bar or something)
        const string overload_text_format = " OVERLOAD: {0:0.00} / {1:0.00} ";

        var new_text = string.Format(overload_text_format, overload_system.load, overload_system.load_limit);
        text_to_replace_by_some_hud_bar.text = new_text;
    }

    private void clear_display()
    {
        text_to_replace_by_some_hud_bar.text = "";
    }

}
