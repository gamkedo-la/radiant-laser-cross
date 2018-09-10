using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    public class LaserCrossGun : MonoBehaviour
    {

        private enum ShootingState
        {
            idle
        , fire
        , resetting
        };
        private ShootingState state = ShootingState.idle;

        void Start()
        {

        }

        void Update()
        {
            // TEMPORARY TO CHECK THAT TRIGGERING FIRING WORKS
            switch (state)
            {
                case ShootingState.fire:
                    state = ShootingState.resetting;
                    break;
                case ShootingState.resetting:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    state = ShootingState.idle;
                    break;
                default:
                    break;
            }
        }


        public void fire()
        {
            //if (state == ShootingState.idle)
            {
                var fire_direction = transform.up;

                transform.localScale = new Vector3(2.0f, 2.0f, 2.0f); // TEMPORARY

                state = ShootingState.fire;
            }
        }

    }
}