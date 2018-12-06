using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Overload Color", menuName ="Overload Color")]
public class UI_OverloadBarColor : ScriptableObject {
    public Color weak;
    public Color medium;
    public Color strong;

    public System.Nullable<Color> stronger_than(Color color)
    {
        if (color == weak) return medium;
        if (color == medium) return strong;
        return null;
    }

    public System.Nullable<Color> weaker_than(Color color)
    {
        if (color == strong) return medium;
        if (color == medium) return weak;
        return null;
    }
}
