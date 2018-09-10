using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    public class LaserCrossControl : MonoBehaviour
    {

        public LaserCross laser_cross;

        void Start()
        {

        }

        private void FixedUpdate()
        {

        }

        void Update()
        {
            Commands commands = read_input();
            laser_cross.push_next_commands(commands);
        }


        private Commands read_input()
        {
            Commands commands = new Commands();

            // The code commented below generate latency between input and move and I'm not sure why:
            //float vertical_move = Input.GetAxis("Vertical");
            //float horizontal_move = Input.GetAxis("Horizontal");

            //commands.ship_direction.y = vertical_move;
            //commands.ship_direction.x = horizontal_move;

            if (Input.GetKey(KeyCode.A))
                commands.ship_direction.x -= 1;
            if (Input.GetKey(KeyCode.D))
                commands.ship_direction.x += 1;
            if (Input.GetKey(KeyCode.W))
                commands.ship_direction.y += 1;
            if (Input.GetKey(KeyCode.S))
                commands.ship_direction.y -= 1;

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.J))
                commands.gun_trigger.fire_west = true;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.L))
                commands.gun_trigger.fire_east = true;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.I))
                commands.gun_trigger.fire_north = true;
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.K))
                commands.gun_trigger.fire_south = true;



            return commands;
        }


    }


}
