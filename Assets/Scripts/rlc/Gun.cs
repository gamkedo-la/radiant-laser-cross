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

        [Tooltip("Objects used as a starting point for emitted bullets, it's forward orientation is used as default bullet direction. Each firing happen in parallel for each emitter. ")]
        public List<Transform> emitters;


        [Tooltip("Bullet Selection mode: how bullet are chosen for each firing.")]
        public GunBulletSelectionMode bullet_selection_mode = GunBulletSelectionMode.random;

        [Tooltip("Time between each bullet prefab change if bullet selection mode allows it.")]
        public float time_between_bullet_change = 0.0f;

        [Tooltip("Fireing mode: how each firing is treated.")]
        public GunFiringMode firing_mode = GunFiringMode.one_shot;


        [Tooltip("Time between each firing.")]
        public float time_between_firing = 1.0f / 32.0f;

        [Tooltip("How many bullets to fire per firing in burst mode.")]
        public int shots_per_burst = 3;

        [Tooltip("Time between each shot (for example in burst mode).")]
        public float time_between_burst_shots = 1.0f / 32.0f;

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
        private float last_shot_time;
        private float last_bullet_change_time;
        private int burst_shots_count = 0;
        private Bullet selected_bullet_prefab;


        private Renderer this_gun_renderer;
        private Seeker seeker;

        private AudioSource sound_fire;


        private enum ShootingState
        {
            idle
        ,   fire
        ,   burst
        ,   resetting
        };

        private ShootingState state = ShootingState.idle;

        void Start()
        {
            last_firing_time = Time.time;
            last_shot_time = last_firing_time;
            this_gun_renderer = GetComponent<Renderer>();
            seeker = GetComponent<Seeker>();
            sound_fire = GetComponent<AudioSource>();
            selected_bullet_prefab = bullet_prefabs[0];
        }

        void Update()
        {
            switch (state)
            {
                case ShootingState.fire:
                    state = ShootingState.resetting;
                    break;

                case ShootingState.burst:
                    var time_since_last_shot = Time.time - last_shot_time;
                    if (time_since_last_shot > time_between_burst_shots)
                    {
                        if (burst_shots_count < shots_per_burst)
                        {
                            shoot();
                        }
                        else
                        {
                            burst_shots_count = 0;
                            state = ShootingState.resetting;
                        }
                    }
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
            shoot();
        }

        // Called when we begin the firing state.
        private void start_firing_state()
        {
            if(default_firing_animation)
                transform.localScale = new Vector3(2.0f, 2.0f, 2.0f); // TEMPORARY effect

            last_firing_time = Time.time;

            select_bullet_prefab();

            switch (firing_mode)
            {
                case GunFiringMode.one_shot:
                    state = ShootingState.fire;
                    break;
                case GunFiringMode.burst:
                    state = ShootingState.burst;
                    break;
            }
        }

        // Called when we should get back to a state ready to fire.
        private void reset_to_ready()
        {
            state = ShootingState.idle;
        }

        // Implements how the bullets should be launched.
        private void shoot()
        {
            last_shot_time = Time.time;

            if (firing_mode == GunFiringMode.burst)
            {
                last_firing_time = Time.time; // The time between firing start at the end of the last firing shot.
                ++burst_shots_count;
            }

            foreach (var emitter in emitters)
            {
                var central_firing_direction = emitter.forward;

                if (parallel_shots < 1)
                    parallel_shots = 1;

                if (parallel_shots == 1)
                {
                    emit_bullet(emitter.position, central_firing_direction);
                    return;
                }

                var half_spread_angle = spread_angle_degs / 2.0f;

                var rotation_to_last_direction = Quaternion.Euler(0, 0, half_spread_angle);
                var rotation_to_first_direction = Quaternion.Inverse(rotation_to_last_direction);

                var first_shot_direction = rotation_to_first_direction * central_firing_direction;
                var last_shot_direction = rotation_to_last_direction * central_firing_direction;

                for (int shot_angle_idx = 0; shot_angle_idx < parallel_shots; ++shot_angle_idx)
                {
                    var ratio = (float)shot_angle_idx / (float)(parallel_shots - 1);

                    // TODO: fix this when the angle is >= 180degs
                    var firing_direction = Vector3.Slerp(first_shot_direction, last_shot_direction, ratio);

                    emit_bullet(emitter.position, firing_direction);
                }
            }
        }

        // Emits one bullet.
        // Should be called by whatever is driving the bullet pattern.
        private void emit_bullet(Vector3 position, Vector3 direction)
        {
            Bullet bullet = (Bullet)Instantiate(selected_bullet_prefab, position, transform.rotation);
            bullet.transform.forward = direction;
            bullet.ClanWhoFired = clan;

            // If the gun have a target, make the bullet inherit the same target if it is homing.
            inherit_target(bullet);

            if (default_bullet_speed > 0)
                bullet.speed = default_bullet_speed;
            bullet.gameObject.tag = Bullet.TAG;

            play_sound_fire();
        }

        private void select_bullet_prefab()
        {

            var time_since_last_bullet_change = Time.time - last_bullet_change_time;
            if (time_since_last_bullet_change <= time_between_bullet_change)
                return;

            last_bullet_change_time = Time.time;

            switch (bullet_selection_mode)
            {
                case GunBulletSelectionMode.random:
                    int random_idx = Random.Range(0, bullet_prefabs.Count);
                    selected_bullet_prefab = bullet_prefabs[random_idx];
                    break;
                case GunBulletSelectionMode.top_bullet:
                    selected_bullet_prefab = bullet_prefabs[0];
                    break;
                case GunBulletSelectionMode.rotate:
                    Utility.RotateForward(bullet_prefabs, 1);
                    selected_bullet_prefab = bullet_prefabs[0];
                    break;
            }
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