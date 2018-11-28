using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class UI_Scoring : MonoBehaviour {
    public Text score_text;
    public GameObject kill_points_prefab;
    private const int SCORE_MAX_DIGITS = 8;

    private static UI_Scoring current;

    void Start()
    {
        current = this;
    }

    public static void DisplaySequenceCount(int score)
    {
        var score_str = score.ToString();
        var builder = new StringBuilder();
        for (int i = score_str.Length; i < SCORE_MAX_DIGITS; i++)
            builder.Append("0");

        builder.Append(score_str);
        current.score_text.text = builder.ToString();
    }

    public static void DisplayEnemyPoint(Vector3 screenPosition, int points, string bonusText)
    {
        var killPointsObj = GameObject.Instantiate(current.kill_points_prefab, current.transform.parent.parent);
        var killPoints = killPointsObj.GetComponent<UI_ScoringKillPoints>();
        killPoints.DisplayKill(screenPosition, points, bonusText);
    }
}
