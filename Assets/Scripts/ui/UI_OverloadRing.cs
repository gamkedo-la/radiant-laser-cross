using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OverloadRing : MonoBehaviour {
    public Image fill_image;
    private Image border_image;
    private const float ANIMATION_DURATION = 1f;
    private const float FILL_ALPHA = 0.2f;
    private UI_OverloadBarColor color;
    private float fade_delay = -100f;
    private bool is_blocked = false;

	void Start () {
        border_image = GetComponent<Image>();
	}
	
	void Update () {
        if (is_blocked) return;
        fade_delay -= Time.deltaTime;

        if(fade_delay >= -99f)
        {
            if (fade_delay <= 0f)
            {
                fade_delay = -100f;
            } else if (fade_delay <= ANIMATION_DURATION * 1f / 3f)
            {
                fill_image.color = color.strong;
            } else if (fade_delay <= ANIMATION_DURATION * 1f / 3f * 2f)
            {
                fill_image.color = color.medium;
            } else if (fade_delay <= ANIMATION_DURATION)
            {
                fill_image.color = color.weak;
            }
            border_image.color = color.strong;
            fill_image.color = new Color(fill_image.color.r, border_image.color.g, border_image.color.b, FILL_ALPHA);
            fill_image.gameObject.SetActive(true);
        } else
        {
            border_image.color = UI_NewOverload.REST_COLOR;
            fill_image.gameObject.SetActive(false);
        }
	}

    public void fill(UI_OverloadBarColor color)
    {
        if (this.color == color) return;
        this.color = color;
        fade_delay = ANIMATION_DURATION;
    }

    public void block(float animation_percent)
    {
        this.is_blocked = true;
        var fill_color = ColorUtils.transition(UI_NewOverload.BLOCK_COLOR, UI_NewOverload.REST_COLOR, animation_percent);
        var border_color = ColorUtils.transition(new Color(0f, 0f, 0f), UI_NewOverload.REST_COLOR, animation_percent);
        fill_color = new Color(fill_color.r, fill_color.g, fill_color.b, FILL_ALPHA);

        fill_image.color = fill_color;
        fill_image.gameObject.SetActive(true);
        border_image.color = border_color;
    }

    public void unblock()
    {
        fill_image.gameObject.SetActive(false);
        border_image.color = UI_NewOverload.REST_COLOR;
        this.is_blocked = false;
    }
}
