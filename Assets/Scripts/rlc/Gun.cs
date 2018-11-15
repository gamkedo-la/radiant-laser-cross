using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    public enum GunFiringMode
    {
        one_shot,           // Emit one bullet per shot.
        burst,              // Emit several bullets per shot.
    }

    public enum GunBulletSelectionMode
    {
        random,                 // Chose a bullet prefab randomly at each firing (one bullet type by burst).
        top_bullet,             // Only the bullet prefab at the top will be used. Change it programatically if you want to change it.
        rotate,                 // Use the top bullet prefab for a shot and rotate the bullet prefabs for next shot.
    }

    /* Behaviour of any kind of gun, as in "bullet emitter". */
    public class Gun : MonoBehaviour
    {
        [Tooltip("Prefabs that will be used as bullet, randomly chosen (TODO: add a choice of how to chose them).")]
        public List<Bullet> bullet_prefabs;

        [Tooltip("Object used as a starting point for emitted bullets, it's forward orientation is used as default bullet direction.")]
        public Transform emitter;

        [Tooltip("Fireing mode: how each firing is treated.")]
        GunFiringMode firing_mode = GunFiringMode.one_shot;

        [Tooltip("Bullet Selection mode: how bullet are chosen for each firing.")]
        GunBulletSelectionMode bullet_selection = GunBulletSelectionMode.random;

        [Tooltip("Time between each firing.")]
        public float time_between_firing = 1.0f / 32.0f;

        [Tooltip("Time between each shot (for example in burst mode).")]
        public float time_between_burst_shot = 1.0f / 32.0f;

        [Tooltip("How many bullets to fire at the same time when firing, spread uniformly along the spread range.")]
        public int parallel_shots = 1;

        [Tooltip("Angle around the emitter forward direction (on the game plane) used to spread bullets ")]
        public float spread_angle_degs = 0.0f;

        [Tooltip("Bullet speed applied to all bullets when fired, or their default speed if 0.")]
        public float default_bullet_speed = 0.0f;

        [Tooltip("Determine the clan of the bullets being fired.")]
        public Clan clan = Clan.enemy;

        [Tooltip("Set to true if bullets fired from this gun should be able to live outside the screen.")]
        public bool allow_firing_outside_screen = false;



        public bool default_firing_animation = false;

        private float last_firing_time;
        private Renderer this_gun_renderer;
        private Seeker seeker;

        private AudioSource sound_fire;

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
            sound_fire = GetComponent<AudioSource>();
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
            var central_firing_direction = emitter.forward;

            if (parallel_shots < 1)
                parallel_shots = 1;

            if (parallel_shots == 1)
            {
                emit_bullet(central_firing_direction);
                return;
            }

            var half_spread_angle = spread_angle_degs / 2.0f;

            var rotation_to_last_direction = Quaternion.Euler(0, 0, half_spread_angle);
            var rotation_to_first_direction = Quaternion.Inverse(rotation_to_last_direction);

            var first_shot_direction = rotation_to_first_direction * central_firing_direction;
            var last_shot_direction = rotation_to_last_direction * central_firing_direction;

            for (int shot_idx = 0; shot_idx < parallel_shots; ++shot_idx)
            {
                var ratio = (float)shot_idx / (float)(parallel_shots - 1);

                // Note: The following didn't work but I don't know why.
                //// Using Slerp can give weird results over 180degs spread angle, so we'll force lerp on a plane.
                //var firing_direction_2d = Vector2.Lerp(new Vector2(first_shot_direction.x, first_shot_direction.y), new Vector2(last_shot_direction.x, last_shot_direction.y), ratio);
                //var firing_direction = new Vector3(firing_direction_2d.x, firing_direction_2d.y);

                var firing_direction = Vector3.Slerp(first_shot_direction, last_shot_direction, ratio);

                emit_bullet(firing_direction);
            }
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

            play_sound_fire();
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

        private void play_sound_fire()
        {
            if (sound_fire)
            {
                sound_fire.Play();
            }
        }


    }
}