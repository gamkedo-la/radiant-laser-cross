using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FancyText : MonoBehaviour {
    private Text front;
    private Text back;
    private float dissipateOffset = 0f;
    
	void Start ()
    {
        CaptureComponents();
    }

    private void CaptureComponents()
    {
        back = GetComponent<Text>();
        front = transform.GetChild(0).GetComponentInChildren<Text>();
    }

    public string text {
        set
        {
            if (back == null) CaptureComponents();
            if (back == null) return;
            back.text = value;

            if (front == null) return;
            front.text = value;
        }
    }

    public float alpha
    {
        set
        {
            back.color = new Color(back.color.r, back.color.g, back.color.b, value);
            front.color = new Color(front.color.r, front.color.g, front.color.b, value);
        }
    }

    public float DissipateOffset
    {
        get { return dissipateOffset; }
        set
        {
            dissipateOffset = value;
            var bpos = back.rectTransform.position;
            back.rectTransform.position = new Vector3(bpos.x - dissipateOffset/2f, bpos.y, bpos.z);
            var fpos = front.rectTransform.position;
            front.rectTransform.position = new Vector3(fpos.x + dissipateOffset, fpos.y, fpos.z);
        }
    }
}
