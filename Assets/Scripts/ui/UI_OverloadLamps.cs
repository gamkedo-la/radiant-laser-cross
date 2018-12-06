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
        this.force_color = color;
    }

    public void change_color(UI_OverloadBarColor color)
    {
        this.color = color;
        force_color = null;
    }

    public void set_intensity(float intensity)
    {
        this.intensity = intensity;
    }
}
