using UnityEngine;
using System.Collections;

namespace rlc
{
    /* Body with a color: can be hit by bullets of the same color.
     *
     **/
    public class ColoredBody : MonoBehaviour
    {
        [Tooltip("Color associated with this object.")]
        public ColorFamily color_family = ColorFamily.Whites;

        [Tooltip("Do not fill: Will be automatically filled by LifeControl itself once this compoenent is listed in it.")]
        public LifeControl life_control;

    }
}