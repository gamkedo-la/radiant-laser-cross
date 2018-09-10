using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace rlc
{

    public class LaserCross : MonoBehaviour
    {
        private enum GunFireDirection : int
        {
            north,
            east,
            south,
            west
        };

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




        private LaserCrossGun[] guns = new LaserCrossGun[GUNS_COUNT];
        private float current_guns_angle = 0.0f;
        private GunsRotation current_guns_rotation = GunsRotation.none;
        private GunFireDirection current_guns_directions = GunFireDirection.north;

        private Commands next_commands;

        public float move_speed = 10.0f;
        public float rotation_speed = 90.0f;

        private LaserCrossGun gun_north()   { return guns[(int)GunFireDirection.north];  }
        private LaserCrossGun gun_east()    { return guns[(int)GunFireDirection.east];   }
        private LaserCrossGun gun_south()   { return guns[(int)GunFireDirection.south];  }
        private LaserCrossGun gun_west()    { return guns[(int)GunFireDirection.west];   }


        void Start()
        {
            guns = GetComponentsInChildren<LaserCrossGun>();
        }

        void Update()
        {
            apply_commands(next_commands);
            clear_commands();
        }

        public void push_next_commands(Commands commands)
        {
            next_commands = commands;
        }


        private void apply_commands(Commands commands)
        {
            Vector2 translation = commands.ship_direction.normalized * move_speed * Time.deltaTime;
            transform.Translate(translation, Space.World);

            if (commands.gun_trigger.fire_north)
                gun_north().fire();
            if (commands.gun_trigger.fire_east)
                gun_east().fire();
            if (commands.gun_trigger.fire_south)
                gun_south().fire();
            if (commands.gun_trigger.fire_west)
                gun_west().fire();

            rotate_guns(commands.gun_move);
        }

        private void clear_commands()
        {
            next_commands = new Commands();
        }

        private void rotate_guns(GunsRotation rotation_from_commands)
        {
            // We want rotations to always happen in lock-steps of axis,
            // that is, rotation can only change when guns are aligned with the axis.
            // If the player maintain a rotation direction, we should seemlessly continue
            // rotating until they don't, then we stop rotation once reaching an axis orientation.

            if (current_guns_rotation == GunsRotation.none) // If we are already in locked position, take any command, otherwise ignore commands until next lock position.
            {
                current_guns_rotation = rotation_from_commands;
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
        }

        private static GunFireDirection next_axis(float current_angle, GunsRotation rotation)
        {
            if (rotation == GunsRotation.none)
                return direction(current_angle);

            if (rotation == GunsRotation.rotate_clockwise)
            {
                if (current_angle > angle(GunFireDirection.west))
                    return GunFireDirection.north;
                if (current_angle > angle(GunFireDirection.south))
                    return GunFireDirection.west;
                if (current_angle > angle(GunFireDirection.east))
                    return GunFireDirection.south;
                return GunFireDirection.east;
            }
            else
            {
                if (current_angle < angle(GunFireDirection.east))
                    return GunFireDirection.north;
                if (current_angle < angle(GunFireDirection.south))
                    return GunFireDirection.east;
                if (current_angle < angle(GunFireDirection.west))
                    return GunFireDirection.south;
                return GunFireDirection.west;
            }
        }


        private static float angle(GunFireDirection direction)
        {
            switch (direction)
            {
                case GunFireDirection.north:
                    return 0.0f;
                case GunFireDirection.east:
                    return GUNS_DEGREES_PER_DIRECTION;
                case GunFireDirection.south:
                    return GUNS_DEGREES_PER_DIRECTION * 2;
                case GunFireDirection.west:
                    return GUNS_DEGREES_PER_DIRECTION * 3;
                default:
                    Assert.IsTrue(false);
                    return 0.0f;
            }
        }

        private static GunFireDirection direction(float angle_deg)
        {
            Assert.IsTrue(angle_deg >= 0);
            Assert.IsTrue(angle_deg <= MAX_GUNS_ORIENTATION_DEGREES);


            if (angle_deg >= left_side_north_begin_angle || angle_deg < right_side_north_end_angle)
                return GunFireDirection.north;

            if (angle_deg >= begin_east_angle && angle_deg < end_east_angle)
                return GunFireDirection.east;

            if (angle_deg >= begin_south_angle && angle_deg < end_south_angle)
                return GunFireDirection.south;

            if (angle_deg >= begin_west_angle && angle_deg < end_west_angle)
                return GunFireDirection.west;

            Assert.IsTrue(false);
            return GunFireDirection.north;
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
    }
}