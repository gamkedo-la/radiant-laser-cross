using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoringMultiplier : MonoBehaviour {
    public UI_FancyText multiplier_text;
    public GameObject sequence_count_wrapper;
    public UI_MultiplierPoint[] sequence_images;

    private static UI_ScoringMultiplier current;

    void Start () {
        current = this;
        sequence_images = sequence_count_wrapper.GetComponentsInChildren<UI_MultiplierPoint>();
        DisplayMultiplier(1);
    }

    public static void DisplayMultiplier(int multiplier)
    {
        if (multiplier > 1)
        {
            current.multiplier_text.gameObject.SetActive(true);
            current.multiplier_text.text = "x" + multiplier.ToString();
        }
        else
        {
            current.multiplier_text.gameObject.SetActive(false);
        }
    }

    public static void DisplaySequenceCount(int number)
    {
        for (int i = 0; i < number; i++)
            current.sequence_images[i].fill();

        for (int i = number; i < 5; i++)
            current.sequence_images[i].empty();
    }
}
