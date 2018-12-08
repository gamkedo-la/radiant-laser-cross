using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MultiplierPoint : MonoBehaviour {
    private GameObject fill_image;

    void Start () {
        fill_image = transform.GetChild(0).gameObject;
	}

    public void fill()
    {
        fill_image.SetActive(true);
    }

    public void empty()
    {
        fill_image.SetActive(false);
    }
}
