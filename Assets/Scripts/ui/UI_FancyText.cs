using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FancyText : MonoBehaviour {
    private Text front;
    private Text back;
    
	void Start () {
        back = GetComponent<Text>();
        front = transform.GetChild(0).GetComponentInChildren<Text>();
	}

    public string text {
        set
        {
            back.text = value;
            front.text = value;
        }
    }
}
