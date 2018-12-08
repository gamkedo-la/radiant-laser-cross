using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtils {
    public static Color transition(Color origin, Color destiny, float percent)
    {
        var red = origin.r - (origin.r - destiny.r) * percent;
        var green = origin.g - (origin.g - destiny.g) * percent;
        var blue = origin.b - (origin.b - destiny.b) * percent;
        return new Color(red, green, blue);
    }
}
