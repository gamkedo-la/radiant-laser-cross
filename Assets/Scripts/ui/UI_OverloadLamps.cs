using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_OverloadLamps : MonoBehaviour {
    private Image image;
    private float intensity = 0f;
    private UI_OverloadBarColor color;
    private Nullable<Color> force_color;
    private bool is_blocked = false;

    void Start()
    {
        image = GetComponent<Image>();
        force_color = UI_NewOverload.REST_COLOR;
    }

    void Update()
    {
        if (force_color.HasValue)
        {
            image.color = force_color.Value;
            return;
        }

        if (this.intensity <= 1f / 3f)
        {
            image.color = color.weak;
        }
        else if (this.intensity <= 1f / 3f * 2f)
        {
            image.color = color.medium;
        }
        else
        {
            image.color = color.strong;
        }
    }

    public void change_color(Color color)
    {
        if (is_blocked) return;
        this.force_color = color;
    }

    public void change_color(UI_OverloadBarColor color)
    {
        if (is_blocked) return;
        this.color = color;
        force_color = null;
    }

    public void block(float animation_percent)
    {
        this.is_blocked = true;
        this.force_color = ColorUtils.transition(UI_NewOverload.BLOCK_COLOR, UI_NewOverload.REST_COLOR, animation_percent);
    }

    public void unblock()
    {
        this.is_blocked = false;
        this.force_color = UI_NewOverload.REST_COLOR;
    }

    public void set_intensity(float intensity)
    {
        this.intensity = intensity;
    }
}
