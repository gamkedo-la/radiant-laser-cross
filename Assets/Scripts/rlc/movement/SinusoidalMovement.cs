using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    public class SinusoidalMovement : IMoving
    {
        private IMoving baseMovement;

        public float frequency = 3.0f;  // Speed of sine movement
        public float magnitude = 0.5f;   // Size of sine movement

        private Vector3 axis;

        public SinusoidalMovement(IMoving baseMovement, AxesDirections axis)
        {
            this.baseMovement = baseMovement;

            switch (axis)
            {
                case AxesDirections.north: this.axis = Vector3.right; break;
                case AxesDirections.south: this.axis = Vector3.left; break;
                case AxesDirections.east: this.axis = Vector3.up; break;
                case AxesDirections.west: this.axis = Vector3.down; break;
            }
        }

        public void Move(Transform transform, float speed)
        {
            Move(transform, axis, speed);
        }

        public void Move(Transform transform, Vector3 axis, float speed)
        {
            baseMovement.Move(transform, speed);
            transform.Translate( (transform.forward * Time.deltaTime * speed) + (axis * Mathf.Sin(Time.time * frequency) * magnitude), Space.World);
        }
    }
}