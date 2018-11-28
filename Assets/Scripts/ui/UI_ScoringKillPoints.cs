using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoringKillPoints : MonoBehaviour {
    public Text time_bonus_text;
    public Text points_text;
    private RectTransform rect;
    private float time_past = 0;

    public void Awake()
    {
        this.rect = GetComponent<RectTransform>();
    }

    public void Update()
    {
        time_past += Time.deltaTime;
        var pos = this.rect.position;
        this.rect.position = new Vector3(pos.x, pos.y + Time.deltaTime*5f, pos.z);
        if(time_past >= 1f)
        {
            Destroy(this.gameObject);
        }
    }

    public void DisplayKill(Vector3 position, int points, string bonusText)
    {
        this.transform.position = position - new Vector3(this.rect.rect.width/2f, 0f, 0f);
        if (bonusText.Length == 0)
        {
            time_bonus_text.text = "";
        } else
        {
            time_bonus_text.text = bonusText + "!";
        }
        points_text.text = points.ToString();
    }
}
