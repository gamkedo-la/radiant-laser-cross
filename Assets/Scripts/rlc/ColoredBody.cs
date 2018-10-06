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

        [Tooltip("True if part of the player's body. Used to differentiate hitting the player vs hitting an ennemy")]
        public bool is_player = false;

        [Tooltip("Do not fill: Will be automatically filled by LifeControl itself once this compoenent is listed in it.")]
        public LifeControl life_control;


        public void on_hit()
        {
            // TODO: insert here an animation specific to this body when hit
            life_control.on_hit();
        }
    }
}