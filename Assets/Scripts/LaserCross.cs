using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    public class LaserCross : MonoBehaviour
    {

        private Commands next_commands;

        public float speed = 1.0f;

        void Start()
        {

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
            Vector2 translation = commands.ship_direction.normalized * speed * Time.deltaTime;
            transform.Translate(translation);
            // TODO: shooting and rotating guns
        }

        private void clear_commands()
        {
            next_commands = new Commands();
        }
    }
}