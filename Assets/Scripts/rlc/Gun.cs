using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    public class Gun : MonoBehaviour
    {
        public Bullet bullet_prefab;        // Prefab that will be used as bullet.
        public Transform emitter_point;     // Object used as a starting point for emitted bullets.
        public Transform target_point;      // Object to target when firing.
        public float time_between_firing = 1.0f / 32.0f;

        private float last_firing_time;

        private enum ShootingState
        {
            idle
        ,   fire
        ,   resetting
        };

        private ShootingState state = ShootingState.idle;

        void Start()
        {
            last_firing_time = Time.time;
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
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // TEMPORARY effect
                    var time_since_last_firing = Time.time - last_firing_time;
                    if (time_since_last_firing > time_between_firing)
                        reset_to_ready();
                    break;
                default:
                    break;
            }
        }


        public void fire()
        {
            if (state != ShootingState.idle)
            {
                // Debug.Log("Ignoring firing command because gun is not idle", this);
                return;
            }

            // Debug.Log("FIRE: " + name + " : TODO: add fire info here if necessary");

            // If some timing mechanism should be added, do it here (if it still make sense)
            start_firing_state();
            emit_bullets_in_target_direction();
        }

        private void start_firing_state()
        {
            transform.localScale = new Vector3(2.0f, 2.0f, 2.0f); // TEMPORARY effect
            state = ShootingState.fire;
            last_firing_time = Time.time;
        }

        private void reset_to_ready()
        {
            state = ShootingState.idle;
        }

        private void emit_bullets_in_target_direction()
        {
            var fire_direction = target_point.position - emitter_point.position;
            // TODO BEWARE: For the moment we assume that there is only one bullet - in the future change the bullet number and pattern from here.
            emit_bullet(fire_direction);
        }

        private void emit_bullet(Vector3 direction)
        {
            Bullet bullet = (Bullet)Instantiate(bullet_prefab, emitter_point.position, transform.rotation);
            bullet.set_direction(direction);
        }

    }
}