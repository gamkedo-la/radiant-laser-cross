using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rlc;

public class TimeoutFakeStarter : MonoBehaviour {
 
	void Start () {
        var timeout = GetComponent<TimeoutSystem>();
        timeout.start(90f);
	}
	
}
