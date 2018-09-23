using UnityEngine;
using System.Collections;

namespace rlc
{
    /* Exposes information about the colors of the object, from the game's rules perspective.
     * Also provides callbacks to call when one of the color matching
     * mechanisms is triggered.
     **/
    public class Coloration : MonoBehaviour
    {
        // Color associated with this object.
        public ColorFamily color_family = ColorFamily.Whites;

        // Called (if set) when a bullet with a matching color hits this object.
        public delegate void on_hit_by_matching_bullet(Bullet bullet);
    }
}