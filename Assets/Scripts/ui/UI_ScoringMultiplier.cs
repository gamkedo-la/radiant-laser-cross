using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoringMultiplier : MonoBehaviour {
    public Text multiplier_text;
    public Text sequence_count_text;

    private static UI_ScoringMultiplier current;

	void Start () {
        current = this;
	}

    public static void DisplayMultiplier(int multiplier)
    {
        current.multiplier_text.text = multiplier.ToString() + "x";
    }

    public static void DisplaySequenceCount(int number)
    {
        var builder = new StringBuilder();
        for(int i = 0; i < number; i++)
            builder.Append(".");
        Debug.Log(builder.ToString());
        current.sequence_count_text.text = builder.ToString();
    }
}
