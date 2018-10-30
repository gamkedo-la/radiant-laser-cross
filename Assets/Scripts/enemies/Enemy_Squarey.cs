using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Squarey : MonoBehaviour {

    public float speed = 5.0f;

    private List<rlc.Gun> guns = new List<rlc.Gun>();

    // Use this for initialization
    void Start () {
        guns.AddRange(GetComponentsInChildren<rlc.Gun>());
    }

    // Update is called once per frame
    void Update () {
        rlc.Movement.move_forward(transform, speed);

        foreach (var gun in guns)
        {
            gun.fire();
        }
    }
}
