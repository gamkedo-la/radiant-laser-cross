using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace rlc
{
    public enum AxesDirections : int
    {
        north,
        east,
        south,
        west
    };

    /* Behavior of the Laser Cross, the player's ship.
     */
    public class LaserCross : MonoBehaviour
    {
        public static LaserCross current;

        public const int GUNS_COUNT = 4;
        public const float MAX_GUNS_ORIENTATION_DEGREES = 360.0f;
        public const float GUNS_DEGREES_PER_DIRECTION = MAX_GUNS_ORIENTATION_DEGREES / GUNS_COUNT;
        public const float HALF_GUNS_DEGREES_PER_DIRECTION = GUNS_DEGREES_PER_DIRECTION / 2;
        // Keep in mind north direction is between the highest and lowest values.
        private const float begin_east_angle = HALF_GUNS_DEGREES_PER_DIRECTION;
        private const float end_east_angle = begin_east_angle + GUNS_DEGREES_PER_DIRECTION;
        private const float begin_south_angle = end_east_angle;
        private const float end_south_angle = begin_south_angle + GUNS_DEGREES_PER_DIRECTION;
        private const float begin_west_angle = end_south_angle;
        private const float end_west_angle = begin_west_angle + GUNS_DEGREES_PER_DIRECTION;
        private const float left_side_north_begin_angle = end_west_angle;
        private const float right_side_north_end_angle = begin_east_angle;

        private Gun[] guns = new Gun[GUNS_COUNT];
        private Shield[] shields = new Shield[GUNS_COUNT];
        private float current_guns_angle = 0.0f;
        private GunsRotation current_guns_rotation = GunsRotation.none;
        private AxesDirections current_guns_directions = AxesDirections.north;

        private Commands next_commands;

        public float move_speed = 10.0f;
        public float rotation_speed = 90.0f;
        public LifeControl life_control;
        public OverloadSystem overload_system;

        public AudioSource sound_rotation;
        public AudioSource sound_rotation_stop;
        public AudioSource sound_destroyed;

        // Note that these are functions because the gun on each direction will change while playing.
        private Gun get_gun(AxesDirections direction)   { return guns[(int)direction];  }
        private Shield get_shield(AxesDirections direction) { return shields[(int)direction]; }
        private Movable movable;


        private ProceduralLevelBuilder level_builder;

        private TrailRenderer trails;

        private GameOverReason game_over_reason = GameOverReason.destroyed;

        void Start()
        {
            movable = GetComponentInParent<Movable>();
            level_builder = GameObject.Find("GameSystem").GetComponent<ProceduralLevelBuilder>();

            guns = GetComponentsInChildren<Gun>(true);
            shields = GetComponentsInChildren<Shield>(true);

            trails = GetComponent<TrailRenderer>();

            if (level_builder == null)
            {
                Debug.LogError("No level builder found");
            }

            current = this;
        }

        void FixedUpdate()
        {

            if (life_control.is_alive() && next_commands.ready)
            {
                apply_commands(next_commands);
                clear_commands();
            }

            animate_todo_please_replace_me();
        }

        public void push_next_commands(Commands commands)
        {
            next_commands = commands;
        }

        public void die(GameOverReason reason)
        {
            game_over_reason = reason;
            life_control.die();
        }

        private void animate_todo_please_replace_me()
        {
            if (life_control.is_alive())
            {
                if (overload_system.state == OverloadSystem.State.recovering) // TODO: replace this check by an event
                {
                    animate_on_recovering();
                }
                else
                {
                    animate_ready();
                }
            }
            else
            {
                animate_death();
            }
        }

        private void animate_on_recovering() // TODO: this is a temporary animation, replace this by something more appropriate!
        {
            trails.enabled = false;
        }

        private void animate_ready() // TODO: this is a temporary animation, replace this by something more appropriate!

        {
            trails.enabled = true;
        }

        private void animate_death() // TODO: this is a temporary death animation, replace this by something more appropriate!
        {
            transform.localScale = transform.localScale * 1.05f;
            transform.rotation = transform.rotation * Quaternion.Euler(151.75f * Time.deltaTime, 900.0f * Time.deltaTime, 733.33f * Time.deltaTime);

            if (sound_destroyed && !sound_destroyed.isPlaying)
            {
                sound_destroyed.Play();
            }
        }


        private void apply_commands(Commands commands)
        {
            movable.MoveTowards(commands.ship_direction.normalized, move_speed);

            // Don't get out of the screen
            float pos_x = Mathf.Clamp(transform.position.x, -GameCamera.SIZE_PER_HALF_SIDE, GameCamera.SIZE_PER_HALF_SIDE);
            float pos_y = Mathf.Clamp(transform.position.y, -GameCamera.SIZE_PER_HALF_SIDE, GameCamera.SIZE_PER_HALF_SIDE);
            transform.position = new Vector3(pos_x, pos_y, 0.0f);

            foreach (var shield in shields)
                shield.deactivate();

            // We can use shield and guns only if not overloading!
            if (overload_system.state != OverloadSystem.State.recovering)
            {
                if (!commands.shield_mode)
                {
                    if (commands.gun_trigger.fire_north) fire_gun(AxesDirections.north);
                    if (commands.gun_trigger.fire_east) fire_gun(AxesDirections.east);
                    if (commands.gun_trigger.fire_south) fire_gun(AxesDirections.south);
                    if (commands.gun_trigger.fire_west) fire_gun(AxesDirections.west);
                }
                else
                {
                    if (commands.gun_trigger.fire_north) activate_shield(AxesDirections.north);
                    if (commands.gun_trigger.fire_east) activate_shield(AxesDirections.east);
                    if (commands.gun_trigger.fire_south) activate_shield(AxesDirections.south);
                    if (commands.gun_trigger.fire_west) activate_shield(AxesDirections.west);
                }
            }

            // We can always rotate for free.
            rotate_guns(commands.gun_move);
        }

        private void fire_gun(AxesDirections direction)
        {
            get_gun(direction).fire();
            overload_system.add_load();
        }

        private void activate_shield(AxesDirections direction)
        {
            get_shield(direction).activate();
            //overload_system.add_load();
        }

        private void clear_commands()
        {
            next_commands = new Commands();
        }

        private void rotate_guns(GunsRotation rotation_from_commands)
        {
            // We want rotations to always happen in lock-steps of axis,
            // that is, rotation can only change/start when guns are aligned with the axis.
            // If the player maintain a rotation direction, we should seemlessly continue
            // rotating until they don't, then we stop rotation once reaching an axis orientation.

            if (current_guns_rotation == GunsRotation.none) // If we are already in locked position, take any command, otherwise ignore commands until next lock position.
            {
                current_guns_rotation = rotation_from_commands;
                if (rotation_from_commands != GunsRotation.none)
                {
                    start_rotation_sound();
                }
            }

            float rotation_factor = gun_rotation_factor(current_guns_rotation);
            float next_orientation = (current_guns_angle + rotation_speed * Time.deltaTime * rotation_factor);

            while (next_orientation > MAX_GUNS_ORIENTATION_DEGREES)
                next_orientation -= MAX_GUNS_ORIENTATION_DEGREES;
            while (next_orientation < 0)
                next_orientation += MAX_GUNS_ORIENTATION_DEGREES;

            var new_rotation_direction = direction(next_orientation);

            var new_target_axis = next_axis(next_orientation, current_guns_rotation);
            var current_target_axis = next_axis(current_guns_angle, current_guns_rotation);
            if (new_target_axis != current_target_axis)
            {
                // We are passing over/through a lock position.
                // We either stop here or change direction following commands.
                // It all depends on what is the command from input.
                var previous_gun_rotation = current_guns_rotation;
                current_guns_rotation = rotation_from_commands;
                if (current_guns_rotation == GunsRotation.none || current_guns_rotation != previous_gun_rotation) // TODO: check if the second test isn't enough for both cases
                {
                    // We need to stop: lock the position to align with axis.
                    next_orientation = angle(current_target_axis);
                    stop_rotation_sound();
                    play_rotation_stop_sound();
                }
            }

            // Handle gun firing rotation: the firing direction gives the closest gun to that direction to fire.
            if (new_rotation_direction != current_guns_directions)
            {
                // We will change of gun direction.
                rotate_guns_directions((int)rotation_factor);
                current_guns_directions = new_rotation_direction;
            }

            // Apply guns rotation.
            current_guns_angle = next_orientation;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, -current_guns_angle); // Because of Z being oriented this way in Unity, we need negative values to rotate clockwise.
        }

        private void rotate_guns_directions(int rotation_steps)
        {
           Utility.Rotate(guns, rotation_steps);
           Utility.Rotate(shields, rotation_steps);
        }


        private void start_rotation_sound()
        {
            if (sound_rotation)
            {
                sound_rotation.Play();
            }
        }

        private void stop_rotation_sound()
        {
            if (sound_rotation)
            {
                sound_rotation.Stop();
            }
        }

        private void play_rotation_stop_sound()
        {
            if (sound_rotation_stop)
            {
                sound_rotation_stop.Play();
            }
        }

        private static AxesDirections next_axis(float current_angle, GunsRotation rotation)
        {
            if (rotation == GunsRotation.none)
                return direction(current_angle);

            if (rotation == GunsRotation.rotate_clockwise)
            {
                if (current_angle > angle(AxesDirections.west))
                    return AxesDirections.north;
                if (current_angle > angle(AxesDirections.south))
                    return AxesDirections.west;
                if (current_angle > angle(AxesDirections.east))
                    return AxesDirections.south;
                return AxesDirections.east;
            }
            else
            {
                if (current_angle < angle(AxesDirections.east))
                    return AxesDirections.north;
                if (current_angle < angle(AxesDirections.south))
                    return AxesDirections.east;
                if (current_angle < angle(AxesDirections.west))
                    return AxesDirections.south;
                return AxesDirections.west;
            }
        }


        private static float angle(AxesDirections direction)
        {
            switch (direction)
            {
                case AxesDirections.north:
                    return 0.0f;
                case AxesDirections.east:
                    return GUNS_DEGREES_PER_DIRECTION;
                case AxesDirections.south:
                    return GUNS_DEGREES_PER_DIRECTION * 2;
                case AxesDirections.west:
                    return GUNS_DEGREES_PER_DIRECTION * 3;
                default:
                    Assert.IsTrue(false);
                    return 0.0f;
            }
        }

        private static AxesDirections direction(float angle_deg)
        {
            Assert.IsTrue(angle_deg >= 0);
            Assert.IsTrue(angle_deg <= MAX_GUNS_ORIENTATION_DEGREES);


            if (angle_deg >= left_side_north_begin_angle || angle_deg < right_side_north_end_angle)
                return AxesDirections.north;

            if (angle_deg >= begin_east_angle && angle_deg < end_east_angle)
                return AxesDirections.east;

            if (angle_deg >= begin_south_angle && angle_deg < end_south_angle)
                return AxesDirections.south;

            if (angle_deg >= begin_west_angle && angle_deg < end_west_angle)
                return AxesDirections.west;

            Assert.IsTrue(false);
            return AxesDirections.north;
        }

        // returns 1.0 if going clockwise, -1.0 if going counter-clockwise, 0 if no rotation.
        private static float gun_rotation_factor(GunsRotation rotation_direction)
        {
            switch (rotation_direction)
            {
                case GunsRotation.rotate_clockwise:
                    return 1.0f;
                case GunsRotation.rotate_counter_clockwise:
                    return -1.0f;
                default:
                    return 0.0f;
            }
        }


        private void OnDestroy()
        {
            if (current == this)
                current = null;

            level_builder.game_over(game_over_reason);
        }
    }

}