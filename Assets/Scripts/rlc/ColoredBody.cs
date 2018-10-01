using UnityEngine;
using System.Collections;

namespace rlc
{
    /* Body with a color: can be hit by bullets of the same color.
     *
     **/
    public class ColoredBody : MonoBehaviour
    {
        // Color associated with this object.
        public ColorFamily color_family = ColorFamily.Whites;

    }
}