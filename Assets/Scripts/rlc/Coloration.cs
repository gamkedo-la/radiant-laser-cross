using UnityEngine;
using System.Collections;

namespace rlc
{
    /* Exposes information about the colors of the object,
     * from the game's rules perspective.
     * TODO: add here callbacks to be called when some color
     * matching stuffs happen.
     **/
    public class Coloration : MonoBehaviour
    {
        // Color associated with this object.
        public ColorFamily color_family = ColorFamily.Whites;
    }
}