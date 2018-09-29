using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    /* Behaviour of any kind of gun, as in "bullet emitter". */
    public class Gun : MonoBehaviour
    {
        public Bullet bullet_prefab;            // Prefab that will be used as bullet.
        public Transform emitter;               // Object used as a starting point for emitted bullets, it's forward orientation is used as default bullet direction.
        public float time_between_firing = 1.0f / 32.0f;
        public float default_bullet_speed = 0.0f; // Speed applied to bullets, or their default speed if 0.
        public bool default_firing_animation = false;


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
            switch (state)
            {
                case ShootingState.fire:
                    state = ShootingState.resetting;
                    break;

                case ShootingState.resetting:

                    if(default_firing_animation)
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // TEMPORARY effect

                    var time_since_last_firing = Time.time - last_firing_time;
                    if (time_since_last_firing > time_between_firing)
                        reset_to_ready();
                    break;

                default:
                    break;
            }
        }

        // Call this to trigger bulelt firing.
        // Bullets will only go if the firing timing is right.
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

        // Called when we begin the firing state.
        private void start_firing_state()
        {
            if(default_firing_animation)
                transform.localScale = new Vector3(2.0f, 2.0f, 2.0f); // TEMPORARY effect

            state = ShootingState.fire;
            last_firing_time = Time.time;
        }

        // Called when we should get back to a state ready to fire.
        private void reset_to_ready()
        {
            state = ShootingState.idle;
        }

        // Implements how the bullets should be launched.
        private void emit_bullets_in_target_direction()
        {
            var fire_direction = emitter.forward;
            // TODO BEWARE: For the moment we assume that there is only one bullet - in the future change the bullet number and pattern from here.
            emit_bullet(fire_direction);
        }

        // Emits one bullet.
        // Should be called by whatever is driving the bullet pattern.
        private void emit_bullet(Vector3 direction)
        {
            Bullet bullet = (Bullet)Instantiate(bullet_prefab, emitter.position, transform.rotation);
            bullet.transform.forward = direction;
            if (default_bullet_speed > 0)
                bullet.speed = default_bullet_speed;
        }

    }
}