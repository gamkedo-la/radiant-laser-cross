using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    public class LaserCross : MonoBehaviour
    {
        public const int GUNS_COUNT = 4;

        private LaserCrossGun[] guns = new LaserCrossGun[GUNS_COUNT];

        private Commands next_commands;

        public float move_speed = 10.0f;
        public int rotation_angles = 64;
        public int rotation_speed = 1;

        void Start()
        {
            guns = GetComponentsInChildren<LaserCrossGun>();
        }

        public void push_next_commands(Commands commands)
        {
            next_commands = commands;
        }


        void Update()
        {
            apply_commands(next_commands);
            clear_commands();
        }

        private void apply_commands(Commands commands)
        {
            Vector2 translation = commands.ship_direction.normalized * move_speed * Time.deltaTime;
            transform.Translate(translation);

            if (commands.gun_trigger.fire_north)
                guns[0].fire();
            if (commands.gun_trigger.fire_east)
                guns[1].fire();
            if (commands.gun_trigger.fire_south)
                guns[2].fire();
            if (commands.gun_trigger.fire_west)
                guns[3].fire();

            // TODO: rotation
        }

        private void clear_commands()
        {
            next_commands = new Commands();
        }
    }
}