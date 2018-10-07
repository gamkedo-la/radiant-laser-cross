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
        public Clan clan = Clan.enemy;

        [Tooltip("Do not fill: Will be automatically setup by LifeControl itself once this compoenent is listed in it." +
            "Keep null if this object should not be linked to life of the entity")]
        public LifeControl life_control;

        public enum SurfaceEffect
        {
            solid,         // Bullets will stop when colliding this object, even when not matching color.
            not_solid,     // Bullets will not stop when colliding this object except if matching color.
            reflective     // Bullets will be reflected when colliding this object with matching color.
        }

        [Tooltip("Defines what toif objects colliding but not matching this object's color should stop their course.")]
        public SurfaceEffect surface_effect = SurfaceEffect.solid;

        public void on_hit()
        {
            // TODO: insert here an animation specific to this body when hit
            if(life_control)
                life_control.on_hit();
        }
    }
}