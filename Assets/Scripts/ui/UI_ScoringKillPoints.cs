using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoringKillPoints : MonoBehaviour {
    public UI_FancyText time_bonus_text;
    public UI_FancyText points_text;
    private RectTransform rect;
    private const float DISSIPATE_TOTAL_DURATION = 0.2f;
    private const float DISSIPATE_SPEED = 15f;
    private const float GO_UP_ANIMATION_DURATION = 1f;
    private const float GO_UP_ANIMATION_SPEED = 10f;
    private float go_up_duration = 0f;
    private float dissipate_duration = DISSIPATE_TOTAL_DURATION;

    public void Awake()
    {
        this.rect = GetComponent<RectTransform>();
        go_up_duration = GO_UP_ANIMATION_DURATION;
    }

    public void Update()
    {
        go_up_duration -= Time.deltaTime;
        var pos = this.rect.anchoredPosition3D;
        this.rect.anchoredPosition3D = new Vector3(pos.x, pos.y + Time.deltaTime * GO_UP_ANIMATION_SPEED, pos.z);
        if (dissipate_duration <= 0f)
        {
            Destroy(this.gameObject);
        } else if (go_up_duration <= 0f)
        {
            time_bonus_text.DissipateOffset += Time.deltaTime * DISSIPATE_SPEED;
            points_text.DissipateOffset += Time.deltaTime * DISSIPATE_SPEED;
            time_bonus_text.alpha = dissipate_duration / DISSIPATE_TOTAL_DURATION;
            points_text.alpha = dissipate_duration / DISSIPATE_TOTAL_DURATION;
            dissipate_duration -= Time.deltaTime;
        }
    }

    public void DisplayKill(Vector3 position, int points, string bonusText)
    {
        this.rect.anchoredPosition3D = position;
        if (bonusText.Length == 0)
        {
            time_bonus_text.text = "";
        } else
        {
            time_bonus_text.text = bonusText + "!";
        }
        points_text.text = UI_Scoring.FormatScore(points);
    }
}
