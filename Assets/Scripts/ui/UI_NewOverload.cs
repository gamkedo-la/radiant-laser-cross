using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NewOverload : MonoBehaviour {
    private UI_OverloadBar bar;
    public UI_OverloadBarColor[] section_colors;
    private float last_level = 0f;
    private int last_section = 0;
    private int MAX_SECTIONS;
    private float MAX_RELATIVE_LEVEL;

    void Start () {
        MAX_SECTIONS = section_colors.Length / 2;
        MAX_RELATIVE_LEVEL = 100f / MAX_SECTIONS;
        bar = GetComponentInChildren<UI_OverloadBar>();
        OverloadEvents.OnLoadChange += set_overload;
	}

    public UI_OverloadBar Bar { get { return bar; } }

    public void set_overload(float load)
    {
        var level = load * 10;
        int level_section = Mathf.FloorToInt(level / MAX_RELATIVE_LEVEL);
        if (last_section != level_section)
        {
            bar.change_color(section_colors[level_section * 2], section_colors[level_section * 2 + 1]);
        }
        last_section = level_section;
        var is_increasing = level > last_level;
        var relative_level = level % MAX_RELATIVE_LEVEL;
        var ui_level = 60f * (level_section / (MAX_SECTIONS - 1f)) + 40f * (relative_level / MAX_RELATIVE_LEVEL);
        bar.disturbance = Mathf.RoundToInt(50f * (ui_level / 100f) + (is_increasing ? 50f : 0f));
        bar.level = Mathf.RoundToInt(ui_level);
        
        last_level = level;
    }
}
