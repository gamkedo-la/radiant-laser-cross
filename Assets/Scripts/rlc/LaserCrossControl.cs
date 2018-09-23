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

            //float vertical_move = Input.GetAxis("Vertical");
            //float horizontal_move = Input.GetAxis("Horizontal");

            //commands.ship_direction.y = vertical_move;
            //commands.ship_direction.x = horizontal_move;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Keypad4))
                commands.ship_direction.x -= 1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Keypad6))
                commands.ship_direction.x += 1;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Keypad8))
                commands.ship_direction.y += 1;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Keypad5))
                commands.ship_direction.y -= 1;

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.J))
                commands.gun_trigger.fire_west = true;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.L))
                commands.gun_trigger.fire_east = true;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.I))
                commands.gun_trigger.fire_north = true;
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.K))
                commands.gun_trigger.fire_south = true;

            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Keypad7))
                commands.gun_move = GunsRotation.rotate_counter_clockwise;
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Keypad9))
                commands.gun_move = GunsRotation.rotate_clockwise;

            return commands;
        }


    }


}
