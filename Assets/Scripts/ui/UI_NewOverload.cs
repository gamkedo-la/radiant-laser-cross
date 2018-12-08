using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NewOverload : MonoBehaviour {
    private UI_OverloadBar bar;
    private UI_OverloadLamps lamps;
    public UI_OverloadBarColor[] section_colors;
    public Image head_image;
    private UI_OverloadRings rings;
    private float last_level = 0f;
    private int last_section = 99;
    private int MAX_SECTIONS;
    private float MAX_RELATIVE_LEVEL;
    public static Color REST_COLOR = new Color(0xE4/255f, 0xFF/255f, 0xFF/255f);
    public static Color BLOCK_COLOR = new Color(0x24/255f, 0x24/255f, 0x24/255f);

    void Start () {
        MAX_SECTIONS = section_colors.Length / 2;
        MAX_RELATIVE_LEVEL = 100f / MAX_SECTIONS;
        bar = GetComponentInChildren<UI_OverloadBar>();
        lamps = GetComponentInChildren<UI_OverloadLamps>();
        rings = GetComponentInChildren<UI_OverloadRings>();
        OverloadEvents.OnLoadChange += set_overload;
        OverloadEvents.OnBlock += on_overload_block;
        OverloadEvents.OnUnblock += on_overload_unblock;
    }

    public UI_OverloadBar Bar { get { return bar; } }

    public void set_overload(float load)
    {
        var level = load * 10;
        int level_section = Mathf.FloorToInt(level / MAX_RELATIVE_LEVEL);
        if (last_section != level_section)
        {
            bar.change_color(section_colors[level_section * 2], section_colors[level_section * 2 + 1]);
            rings.colors = new UI_OverloadBarColor[]
            {
                section_colors[level_section * 2], section_colors[level_section * 2 + 1]
            };
        }
        last_section = level_section;
        var is_increasing = level > last_level;
        var relative_level = level % MAX_RELATIVE_LEVEL;
        var ui_level = 60f * (level_section / (MAX_SECTIONS - 1f)) + 40f * (relative_level / MAX_RELATIVE_LEVEL);
        bar.disturbance = Mathf.RoundToInt(50f * (ui_level / 100f) + (is_increasing ? 50f : 0f));
        bar.level = Mathf.RoundToInt(ui_level);
        lamps.set_intensity(relative_level / MAX_RELATIVE_LEVEL);
        rings.disturbance = 0.7f * (level/100f) + (is_increasing ? 0.3f : 0f);

        if (load <= 0.01f)
        {
            lamps.change_color(REST_COLOR);
        } else
        {
            lamps.change_color(section_colors[level_section * 2]);
        }

        last_level = level;
    }

    public void on_overload_block(float time_percent)
    {
        bar.block(time_percent);
        lamps.block(time_percent);
        rings.block(time_percent);
        head_image.color = ColorUtils.transition(BLOCK_COLOR, Color.white, time_percent);
    }

    public void on_overload_unblock()
    {
        bar.unblock();
        lamps.unblock();
        rings.unblock();
        head_image.color = Color.white;
    }
}
