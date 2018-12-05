using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class UI_Scoring : MonoBehaviour {
    public Text score_text;
    public GameObject kill_points_prefab;
    public Transform kill_points_ui;
    private const int SCORE_MAX_DIGITS = 8;

    private static UI_Scoring current;

    void Start()
    {
        current = this;
    }

    public static void DisplaySequenceCount(int score)
    {
        if (current == null) return;
        var score_str = score.ToString();
        var builder = new StringBuilder();
        for (int i = score_str.Length; i < SCORE_MAX_DIGITS; i++)
            builder.Append("0");

        builder.Append(score_str);
        current.score_text.text = builder.ToString();
    }

    public static void DisplayEnemyPoint(Vector3 worldPoint, int points, string bonusText)
    {
        if (current == null) return;
        var ui = current.kill_points_ui; // current.transform.parent.parent;
        var killPointsObj = GameObject.Instantiate(current.kill_points_prefab, ui);
        var killPoints = killPointsObj.GetComponent<UI_ScoringKillPoints>();
        var screenPoint = Camera.main.WorldToScreenPoint(worldPoint);
        killPoints.DisplayKill(screenPoint, points, bonusText);
    }
}
