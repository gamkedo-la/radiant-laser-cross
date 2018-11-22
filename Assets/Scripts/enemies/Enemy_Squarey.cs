using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Squarey : MonoBehaviour {

    public float speed = 5.0f;

    private List<rlc.Gun> guns = new List<rlc.Gun>();
    private Movable movable;

    // Use this for initialization
    void Start () {
        this.movable = GetComponentInParent<Movable>();
        guns.AddRange(GetComponentsInChildren<rlc.Gun>());
    }

    // Update is called once per frame
    void Update () {
        foreach (var gun in guns)
        {
            gun.fire();
        }
    }

    private void FixedUpdate()
    {
        movable.MoveForward(speed);
    }
}
