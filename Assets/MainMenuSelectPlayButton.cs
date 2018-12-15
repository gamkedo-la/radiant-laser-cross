using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuSelectPlayButton : MonoBehaviour {

    public GameObject SelectMe;
    
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
        Debug.Log("Main Menu: Clicked Empty Space: giving focus to play button");
        if (SelectMe)
        {
            EventSystem.current.SetSelectedGameObject(SelectMe, null);
        }
    }
}
