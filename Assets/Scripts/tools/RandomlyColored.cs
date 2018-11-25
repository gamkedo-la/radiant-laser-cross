using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to help identify different objects when working with a lot of them generated.
public class RandomlyColored : MonoBehaviour {

    void Start () {

        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (var material in renderer.materials)
            {
                material.color = new Color(Random.value, Random.value, Random.value, Random.value);
            }
        }
    }

}
