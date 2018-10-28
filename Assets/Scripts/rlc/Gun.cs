using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    /* Behaviour of any kind of gun, as in "bullet emitter". */
    public class Gun : MonoBehaviour
    {
        [Tooltip("Prefabs that will be used as bullet, randomly chosen (TODO: add a choice of how to chose them).")]
        public List<Bullet> bullet_prefabs;

        [Tooltip("Object used as a starting point for emitted bullets, it's forward orientation is used as default bullet direction.")]
        public Transform emitter;                           //

        [Tooltip("Time between each firing.")]
        public float time_between_firing = 1.0f / 32.0f;

        [Tooltip("Bullet speed applied to all bullets when fired.")]
        public float default_bullet_speed = 0.0f; // Speed applied to bullets, or their default speed if 0.

        [Tooltip("Determine the clan of the bullets being fired.")]
        public Clan clan = Clan.enemy;

        [Tooltip("Set to true if bullets fired from this gun should be able to live outside the screen.")]
        public bool allow_firing_outside_screen = false;

        public bool default_firing_animation = false;

        private float last_firing_time;
        private Renderer this_gun_renderer;
        private Seeker seeker;

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
            this_gun_renderer = GetComponent<Renderer>();
            seeker = GetComponent<Seeker>();
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

            // Except if allowed, we do not accept firing until we are in screen (aka the renderer is visible)
            if (!allow_firing_outside_screen && this_gun_renderer != null && !this_gun_renderer.isVisible)
            {
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
            int random_idx = Random.Range(0, bullet_prefabs.Count);
            var bullet_prefab = bullet_prefabs[random_idx];

            Bullet bullet = (Bullet)Instantiate(bullet_prefab, emitter.position, transform.rotation);
            bullet.transform.forward = direction;
            bullet.clan_who_fired = clan;

            // If the gun have a target, make the bullet inherit the same target if it is homing.
            inherit_target(bullet);

            if (default_bullet_speed > 0)
                bullet.speed = default_bullet_speed;
            bullet.gameObject.tag = Bullet.TAG;
        }

        private void inherit_target(Bullet bullet)
        {
            if (seeker == null)
                return;

            var bullet_seeker = bullet.GetComponent<Seeker>();
            if (bullet_seeker == null)
                return;

            bullet_seeker.target = seeker.target;
        }

    }
}