using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Squarey : MonoBehaviour {

    public float speed = 5.0f;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        rlc.Movement.move_forward(transform, speed);
    }
}
