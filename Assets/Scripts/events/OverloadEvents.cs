using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadEvents : MonoBehaviour {
    public delegate void LoadChangeAction(float load);
    public static event LoadChangeAction OnLoadChange;

    public static void InvokeOnLoadChange(float load)
    {
        if (OnLoadChange != null) OnLoadChange(load);
    }

}
