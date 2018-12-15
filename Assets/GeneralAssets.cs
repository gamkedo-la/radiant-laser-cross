using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAssets : MonoBehaviour {

    public GameObject spawning_box;

    public static GeneralAssets instance;

    void Start()
    {
        instance = this;
    }


}
