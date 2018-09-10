using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    public enum GunsRotation // Only one can be used at a time.
    {
        none,
        rotate_clockwise,
        rotate_counter_clockwise
    }

    public struct GunTriggerAction // All can be used simultaneously.
    {
        public bool fire_north;
        public bool fire_east;
        public bool fire_south;
        public bool fire_west;
    }


    // Command to execute to control Laser Cross for one update cycle
    public struct Commands
    {
        public Vector2 ship_direction;               // Normalized direction to where the ship needs to go.
        public GunsRotation gun_move;               // Gun rotation to engage.
        public GunTriggerAction gun_trigger;         // Guns to trigger for shooting.
    }


}