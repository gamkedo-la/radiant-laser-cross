using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OverloadRings : MonoBehaviour {
    private UI_OverloadRing[] rings;
    public UI_OverloadBarColor[] colors;
    public float disturbance;
    private float animation_timeout = 0f;
    private const float MAX_TIMEOUT = 0.1f;
    private const float MIN_TIMEOUT = 0.01f;
    private const int MIN_RINGS_AMOUNT = 1;


    void Start () {
        rings = GetComponentsInChildren<UI_OverloadRing>();
	}
	
	void Update () {
        animation_timeout += Time.deltaTime;
        if (disturbance == 0) return;
        if(animation_timeout >= Timeout)
        {
            animation_timeout = 0f;
            if (colors.Length == 0) return;
            var color = colors[Random.Range(0, colors.Length)];
            var ring = rings[rings.Length - 1 - Random.Range(0, RingsAmount)];
            ring.fill(color);
        }
	}

    public void block(float animation_percent)
    {
        foreach(var ring in rings)
        {
            ring.block(animation_percent);
        }
    }

    public void unblock()
    {
        foreach (var ring in rings)
        {
            ring.unblock();
        }
    }

    public float Timeout
    {
        get { return MAX_TIMEOUT - (MAX_TIMEOUT - MIN_TIMEOUT) * disturbance; }
    }

    public int RingsAmount
    {
        get { return MIN_RINGS_AMOUNT + Mathf.CeilToInt((rings.Length - MIN_RINGS_AMOUNT) * disturbance); }
    }
}
