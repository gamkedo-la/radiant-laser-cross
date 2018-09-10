using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    public enum GunMoveAction // Only one can be used at a time.
    {
        none,
        rotate_clockwise,
        rotate_counter_clockwise
    }

    public enum GunTriggerAction // All can be used simultaneously.
    {
        none,
        fire_north,
        fire_east,
        fire_south,
        fire_west,
    }


    // Command to execute to control Laser Cross for one update cycle
    public struct Commands
    {
        const int MAX_MOVE_ACTIONS_PER_ACTION = 2;
        public Vector2 ship_direction;               // Normalized direction to where the ship needs to go.
        public GunMoveAction gun_move;               // Gun rotation to engage.
        public GunTriggerAction gun_trigger;         // Guns to trigger for shooting.
    }


}