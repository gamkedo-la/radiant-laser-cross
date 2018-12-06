using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OverloadBarCell : MonoBehaviour {
    private Image image;

	void Start () {
        image = GetComponent<Image>();
        hide();
	}
	
	public void hide()
    {
        this.gameObject.SetActive(false);
    }

    public void show()
    {
        this.gameObject.SetActive(true);
    }
}
