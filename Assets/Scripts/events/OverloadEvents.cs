using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadEvents : MonoBehaviour {
    public delegate void LoadChangeAction(float load);
    public static event LoadChangeAction OnLoadChange;

    public delegate void BlockAction(float recovery_percent);
    public static event BlockAction OnBlock;

    public delegate void UnblockAction();
    public static event UnblockAction OnUnblock;

    public static void InvokeOnLoadChange(float load)
    {
        if (OnLoadChange != null) OnLoadChange(load);
    }

    public static void InvokeOnBlock(float recovery_percent)
    {
        if (OnBlock != null) OnBlock(recovery_percent);
    }

    public static void InvokeOnUnblock()
    {
        if (OnUnblock!= null) OnUnblock();
    }
}
