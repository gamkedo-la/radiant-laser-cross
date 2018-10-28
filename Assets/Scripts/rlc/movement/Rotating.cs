using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour {

    public float rotation_speed = 45.0f;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.up, rotation_speed * Time.deltaTime);
    }
}
