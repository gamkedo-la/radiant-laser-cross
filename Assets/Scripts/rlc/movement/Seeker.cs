using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    public class Seeker : MonoBehaviour
    {

        public Transform target;

        [Tooltip("Speed of rotation to go to target in degrees.")]
        public float angle_rotation_speed_degs = 90.0f;

        [Tooltip("Max angle different from the parent's direction, to avoid guns that rotate too far.")]
        public float max_angle_from_parent_direction = 0.0f;

        [Tooltip("Set to true: Automatically set the Laser Cross as the target to look for.")]
        public bool auto_target_player = false;

        private bool is_targeting_player = false;

        void Start()
        {
            if (auto_target_player)
            {
                start_targetting_player();
            }
        }

        void Update()
        {
            if (auto_target_player && !is_targeting_player)
            {
                start_targetting_player();
            }
            else if (!auto_target_player && is_targeting_player)
            {
                stop_targeting_player();
            }

            if (target != null)
            {
                seek();
            }
        }

        private void seek()
        {
            var target_direction = Vector3.Normalize(target.position - this.transform.position);
            var next_angle_rotation = angle_rotation_speed_degs * Time.deltaTime;

            var new_direction = Vector3.RotateTowards(transform.forward, target_direction, Mathf.Deg2Rad * next_angle_rotation, 0.0f);
            new_direction = limit_angle_from_parent(new_direction);

            transform.rotation = Quaternion.LookRotation(new_direction);
        }

        private Vector3 limit_angle_from_parent(Vector3 wanted_direction)
        {
            // Limit the rotation relative to the parent if we are orienting a sub-object (like a gun)
            if (transform.parent != null
            && max_angle_from_parent_direction > 0)
            {
                var angle_relative_to_parent_direction = Vector3.Angle(transform.parent.forward, wanted_direction);
                if (angle_relative_to_parent_direction > max_angle_from_parent_direction)
                {
                    wanted_direction = Vector3.RotateTowards(transform.parent.forward, wanted_direction, Mathf.Deg2Rad * max_angle_from_parent_direction, 0.0f);
                }
            }

            return wanted_direction;
        }

        private void start_targetting_player()
        {
            if (LaserCross.current != null)
            {
                is_targeting_player = true;
                target = LaserCross.current.transform;
            }
        }

        private void stop_targeting_player()
        {
            is_targeting_player = false;
            target = null;
        }

    }
}