using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    public enum GunMoveAction // Only one can be simultaneous.
    {
        none,
        rotate_clockwise,
        rotate_counter_clockwise
    }

    public enum GunTriggerAction // All can be simutaneous.
    {
        none,
        fire_north,
        fire_east,
        fire_south,
        fire_west,
    }

    // Combined action of the Laser Cross
    public struct Action
    {
        const int MAX_MOVE_ACTIONS_PER_ACTION = 2;
        public Vector2 movement;
        public GunMoveAction gun_move;
        public GunTriggerAction gun_trigger;
    }

    public class ActionGenerator
    {

    }

}