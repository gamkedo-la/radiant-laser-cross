using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_OverloadBarCell : MonoBehaviour {
    private Image image;
    private const float LIGHT_ON_DURATION = 0.25f;
    private const float LIGHT_OUT_DURATION = 0.75f;
    private float light_out_timeout = -100f;
    private float light_on_timeout = -100f;
    private UI_OverloadBarColor current_color;
    public bool IsLighten { get; set; }

    void Start () {
        image = GetComponent<Image>();
        hide();
    }

    void Update()
    {
        light_on_timeout -= Time.deltaTime;
        light_out_timeout -= Time.deltaTime;
        if (light_on_timeout > -99f && light_on_timeout <= 0f)
        {
            Nullable<Color> next_color = current_color.stronger_than(image.color);
            if (next_color.HasValue)
            {
                image.color = next_color.Value;
                light_on_timeout = LIGHT_ON_DURATION;
            }
            else
            {
                light_on_timeout = -100f;
            }
        }

        else if (light_out_timeout > -99f && light_out_timeout <= 0f)
        {
            Nullable<Color> next_color = current_color.weaker_than(image.color);
            if (next_color.HasValue)
            {
                image.color = next_color.Value;
                light_out_timeout = LIGHT_OUT_DURATION;
            }
            else
            {
                hide();
                light_out_timeout = -100f;
            }
        }
    }

    public void light_on(UI_OverloadBarColor color)
    {
        if (IsLighten && current_color == color) return;
        current_color = color;
        image.color = color.weak;
        light_on_timeout = LIGHT_ON_DURATION / 3f;
        light_out_timeout = -100f;
        show();
    }

    public void light_out(UI_OverloadBarColor color)
    {
        if (!IsLighten || (light_out_timeout >= -99f && current_color == color)) return;
        current_color = color;
        image.color = color.strong;
        light_out_timeout = LIGHT_OUT_DURATION / 3f;
        light_on_timeout = -100f;
        show();
    }

    private void show()
    {
        IsLighten = true;
    }

    private void hide()
    {
        current_color = null;
        IsLighten = false;
        image.color = new Color(1f, 1f, 1f, 0f);
    }
}
