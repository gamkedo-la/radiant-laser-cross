using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rlc;

public class EnemyEvents : MonoBehaviour {
    public delegate void KilledAction(LifeControl life);
    public static event KilledAction OnKilled;
    
    public static void InvokeOnKilled(LifeControl life)
    {
        if (OnKilled != null) OnKilled(life);
    }

}
