using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OverloadBar : MonoBehaviour {
    public int level;
    public int disturbance;
    public UI_OverloadBarColor foreground_color;
    public UI_OverloadBarColor background_color;
    public Image block_overlay_image;
    private UI_OverloadBarCell[] cells;
    private float actual_level = 0f;
    private float disturbance_delay = 0f;
    private const float MAX_VELOCITY = 96f; // 32% per second
    private const float MIN_VELOCITY = 32f; // 8% per second
    private const float MAX_DISTURBANCE_INCREASE = 24f;
    private const float MIN_DISTURBANCE_INCREASE = 8f;
    private const float MAX_RANDOM_RANGE_DISTURBANCE_INCREASE = 66f;
    private const float MIN_RANDOM_RANGE_DISTURBANCE_INCREASE = 12f;
    private const float MAX_DISTURBANCE_RATE = 1f; // each 3 seconds
    private const float MIN_DISTURBANCE_RATE = 0.3f; // each second
    private const float MIN_DIFF_TO_DISTURBANCE = 15f;
    private const float MAX_BLOCK_OVERLAY_ALPHA = 220f/255f;
    private const float MIN_BLOCK_OVERLAY_ALPHA = 100f/255f;


    void Start () {
        level = 0;
        disturbance = 0;
        cells = GetComponentsInChildren<UI_OverloadBarCell>();
        unblock();
	}
	
	void Update ()
    {
        disturbance_delay += Time.deltaTime;
        if (level >= 0.01f && disturbance_delay >= DisturbanceRate && Mathf.Abs(level - actual_level) <= MIN_DIFF_TO_DISTURBANCE)
        {
            apply_disturbance();
            disturbance_delay = 0f;
        }
        step_level();
        update_cells();
    }

    public void change_color(UI_OverloadBarColor foreground, UI_OverloadBarColor background)
    {
        actual_level = 0f;
        update_cells();
        foreground_color = foreground;
        background_color = background;
    }

    public void block(float animation_progress)
    {
        block_overlay_image.gameObject.SetActive(true);
        var c = block_overlay_image.color;
        var alpha = MIN_BLOCK_OVERLAY_ALPHA + (MAX_BLOCK_OVERLAY_ALPHA - MIN_BLOCK_OVERLAY_ALPHA) * (1f - animation_progress);
        block_overlay_image.color = new Color(c.r, c.g, c.b, alpha);
    }

    public void unblock()
    {
        block_overlay_image.gameObject.SetActive(false);
    }

    private void apply_disturbance()
    {
        var disturbance_increase = DisturbanceIncrease - RandomRangeDisturbanceIncrease * Random.Range(0f, 1f);
        actual_level = Mathf.Min(100f, actual_level + disturbance_increase);
    }

    private void step_level()
    {
        if (actual_level < level)
        {
            actual_level = Mathf.Min(level, actual_level + level_change_velocity());
        }
        else if (actual_level > level)
        {
            actual_level = Mathf.Max(level, actual_level - level_change_velocity());
        }
    }

    private void update_cells()
    {
        var cell_level = Mathf.RoundToInt(actual_level * (cells.Length / 100f));
        for(var i = 1; i <= cells.Length; i++)
        {
            var cell = cells[i-1];
            if (i <= cell_level)
            {
                cell.light_on(foreground_color);
            } else
            {
                cell.light_out(background_color);
            }
        }
    }

    private float level_change_velocity()
    {
        var velocity = MIN_VELOCITY + (MAX_VELOCITY - MIN_VELOCITY) * (disturbance / 100f);
        return Time.deltaTime*velocity;
    }

    private float DisturbanceRate
    {
        get { return MAX_DISTURBANCE_RATE - (MAX_DISTURBANCE_RATE - MIN_DISTURBANCE_RATE) * (disturbance / 100f); }
    }

    private float DisturbanceIncrease
    {
        get { return MIN_DISTURBANCE_INCREASE + (MAX_DISTURBANCE_INCREASE - MIN_DISTURBANCE_INCREASE) * (disturbance / 100f); }
    }

    private float RandomRangeDisturbanceIncrease
    {
        get { return MIN_RANDOM_RANGE_DISTURBANCE_INCREASE - (MAX_RANDOM_RANGE_DISTURBANCE_INCREASE - MIN_RANDOM_RANGE_DISTURBANCE_INCREASE) * (disturbance / 100f); }
    }
}
