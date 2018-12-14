using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FancyText : MonoBehaviour {
    public float dissipateOffset = 0f;
    public float dissipateDuration = 0f;
    public static float DISSIPATE_DURATION = 0.2f;
    private Text front;
    private Text back;
    private bool isEnabled = true;

    private Vector3 initial_front_pos;
    private Vector3 initial_back_pos;

    public enum State
    {
        deactivated, active, dissipating
    }
    public State state = State.active;

    private void CaptureComponents()
    {
        back = GetComponent<Text>();
        initial_back_pos = back.rectTransform.position;

        front = transform.GetChild(0).GetComponentInChildren<Text>();
        initial_front_pos = front.rectTransform.position;
    }

    void Start ()
    {
        CaptureComponents();
        activate();
    }

    private void Update()
    {
        if (state == State.dissipating)
        {
            dissipate();
        }
    }

    private void activate()
    {
        if (state != State.active)
        {
            state = State.active;
            alpha = 1.0f;
            back.gameObject.SetActive(true);
            front.gameObject.SetActive(true);

            back.rectTransform.position = initial_back_pos;
            front.rectTransform.position = initial_front_pos;
        }
    }

    private void deactivate()
    {
        if (state != State.deactivated)
        {
            state = State.deactivated;
            back.gameObject.SetActive(false);
            front.gameObject.SetActive(false);

        }
    }

    private void dissipate()
    {
        if (state != State.dissipating)
        {
            state = State.dissipating;
            dissipateDuration = DISSIPATE_DURATION;
            DissipateOffset = 0;
        }

        DissipateOffset += Time.deltaTime * 15f;
        alpha = dissipateDuration / DISSIPATE_DURATION;
        dissipateDuration -= Time.deltaTime;

        if (dissipateDuration <= 0f)
        {
            deactivate();
        }
    }


    public new bool enabled
    {
        get { return isEnabled; }
        set
        {
            isEnabled = value;
            if (isEnabled)
            {
                activate();
            }
            else
            {
                dissipate();
            }
        }
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
