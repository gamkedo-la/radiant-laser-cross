using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuSelectPlayButton : MonoBehaviour {

    public GameObject SelectMe;
    public GameObject SelectMeDuringCredits;
    public GameObject MainMenuObject;

    // Use this for initialization
    void Start () {
        if (SelectMe)
        {
            EventSystem.current.SetSelectedGameObject(SelectMe, null);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MainMenuClickEmptySpace()
    {

        if (MainMenuObject.activeInHierarchy && SelectMe)
        {
            Debug.Log("Main Menu: Clicked Empty Space: giving focus to PLAY button");
            EventSystem.current.SetSelectedGameObject(SelectMe, null);
        }
        else if (SelectMeDuringCredits)
        {
            Debug.Log("Credits: Clicked Empty Space: giving focus to RETURN button");
            EventSystem.current.SetSelectedGameObject(SelectMeDuringCredits, null);
        }

    }
}
