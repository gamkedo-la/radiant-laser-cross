﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class UI_Scoring : MonoBehaviour {
    public UI_FancyText score_text;
    public GameObject kill_points_prefab;
    public Transform kill_points_ui;
    public const int SCORE_MAX_DIGITS = 8;
    private const float WORLD_Z = -20f; // Not sure why is this value, but the kill points appear on the right position with this

    private static UI_Scoring current;

    void Start()
    {
        current = this;
        UI_Scoring.Display(false); // Turn it off only at the beginning, then keep it that way after started.
    }

    public static void Display(bool must_display)
    {
        if (current == null)
            return;

        current.gameObject.SetActive(must_display);
    }

    public static string FormatScore(int score)
    {
        var score_str = score.ToString();
        var builder = new StringBuilder();
        for (int i = score_str.Length; i < SCORE_MAX_DIGITS; i++)
            builder.Append("0");

        builder.Append(score_str);
        return builder.ToString();

    }

    public static void DisplaySequenceCount(int score)
    {
        if (current == null) return;
        current.score_text.text = FormatScore(score);
    }

    public static void DisplayEnemyPoint(Vector3 worldPoint, int points, string bonusText)
    {
        if (current == null) return;
        var ui = current.kill_points_ui; // current.transform.parent.parent;
        var killPointsObj = GameObject.Instantiate(current.kill_points_prefab, ui);
        var killPoints = killPointsObj.GetComponent<UI_ScoringKillPoints>();
        var screenPoint = Camera.main.WorldToScreenPoint(new Vector3(worldPoint.x, worldPoint.y, WORLD_Z));
        killPoints.DisplayKill(screenPoint, points, bonusText);
    }
}
