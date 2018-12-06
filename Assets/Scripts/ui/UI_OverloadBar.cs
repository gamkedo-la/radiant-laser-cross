using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OverloadBar : MonoBehaviour {
    public int level;
    public int disturbance;
    public UI_OverloadBarColor foreground_color;
    public UI_OverloadBarColor background_color;
    private UI_OverloadBarCell[] cells;
    private float actual_level = 0f;
    private float disturbance_delay = 0f;
    private const float MAX_VELOCITY = 96f; // 32% per second
    private const float MIN_VELOCITY = 32f; // 8% per second
    private const float MAX_DISTURBANCE_DECREASE = 24f;
    private const float MIN_DISTURBANCE_DECREASE = 8f;
    private const float MAX_RANDOM_RANGE_DISTURBANCE_DECREASE = 66f;
    private const float MIN_RANDOM_RANGE_DISTURBANCE_DECREASE = 12f;
    private const float MAX_DISTURBANCE_RATE = 1f; // each 3 seconds
    private const float MIN_DISTURBANCE_RATE = 0.3f; // each second
    private const float MIN_DIFF_TO_DISTURBANCE = 15f;


    void Start () {
        level = 100;
        disturbance = 100;
        cells = GetComponentsInChildren<UI_OverloadBarCell>();
	}
	
	void Update ()
    {
        disturbance_delay += Time.deltaTime;
        if (disturbance_delay >= DisturbanceRate && Mathf.Abs(level - actual_level) <= MIN_DIFF_TO_DISTURBANCE)
        {
            apply_disturbance();
            disturbance_delay = 0f;
        }
        step_level();
        update_cells();
    }

    private void apply_disturbance()
    {
        var disturbance_decrease = DisturbanceDecrease - RandomRangeDisturbanceDecrease * Random.Range(0f, 1f);
        actual_level = Mathf.Max(0f, actual_level - disturbance_decrease);
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
        var cell_level = Mathf.RoundToInt(actual_level * ((cells.Length - 1) / 100f));
        for(var i = 0; i < cells.Length; i++)
        {
            var cell = cells[i];
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

    private float DisturbanceDecrease
    {
        get { return MIN_DISTURBANCE_DECREASE + (MAX_DISTURBANCE_DECREASE - MIN_DISTURBANCE_DECREASE) * (disturbance / 100f); }
    }

    private float RandomRangeDisturbanceDecrease
    {
        get { return MIN_RANDOM_RANGE_DISTURBANCE_DECREASE - (MAX_RANDOM_RANGE_DISTURBANCE_DECREASE - MIN_RANDOM_RANGE_DISTURBANCE_DECREASE) * (disturbance / 100f); }
    }
}
