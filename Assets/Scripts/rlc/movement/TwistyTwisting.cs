using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistyTwisting : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        // Copy pasted from the "temporary" death animation of the laser cross, then tweaked to be realy random.
        transform.rotation = transform.rotation * Quaternion.Euler(Random.Range(1.0f, 1000.0f) * Time.deltaTime, Random.Range(1.0f, 1000.0f) * Time.deltaTime, Random.Range(1.0f, 1000.0f) * Time.deltaTime);
    }
}
