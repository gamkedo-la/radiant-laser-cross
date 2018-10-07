using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    /* Player Controls for the Laser Cross.
     * This is separate to avoid mixing how we get the input from
     * how we apply the player's commands.
     */
    public class LaserCrossControl : MonoBehaviour
    {

        public LaserCross laser_cross;

        void Update()
        {
            Commands commands = read_input();
            laser_cross.push_next_commands(commands);
        }


        private Commands read_input()
        {
            Commands commands = new Commands();

            // The code commented below generate latency between input and move and I'm not sure why:
            // OK Chris said I should use GetAxisRaw instead and it works but then I'm not sure
            // How to setup the rotations and 4 guns firing in Unity.

            float vertical_move = Input.GetAxisRaw("Vertical");
            float horizontal_move = Input.GetAxisRaw("Horizontal");
            float fire_north = Input.GetAxisRaw("Fire Up");
            float fire_east = Input.GetAxisRaw("Fire Right");
            float fire_south = Input.GetAxisRaw("Fire Down");
            float fire_west = Input.GetAxisRaw("Fire Left");
            float rotate_clockwise = Input.GetAxisRaw("Rotate Clockwise");
            float rotate_counterclockwise = Input.GetAxisRaw("Rotate Counter-Clockwise");
            float shield_mode = Input.GetAxisRaw("Shield");

            if (vertical_move > 0)
                commands.ship_direction.y = +1;
            if (vertical_move < 0)
                commands.ship_direction.y = -1;

            if (horizontal_move > 0)
                commands.ship_direction.x = +1;
            if (horizontal_move < 0)
                commands.ship_direction.x = -1;

            if (fire_west > 0)
                commands.gun_trigger.fire_west = true;
            if (fire_east > 0)
                commands.gun_trigger.fire_east = true;
            if (fire_north > 0)
                commands.gun_trigger.fire_north = true;
            if (fire_south > 0)
                commands.gun_trigger.fire_south = true;

            if (rotate_counterclockwise > 0)
                commands.gun_move = GunsRotation.rotate_counter_clockwise;
            if (rotate_clockwise > 0)
                commands.gun_move = GunsRotation.rotate_clockwise;

            if (shield_mode > 0)
                commands.shield_mode = true;

            return commands;
        }


    }


}
