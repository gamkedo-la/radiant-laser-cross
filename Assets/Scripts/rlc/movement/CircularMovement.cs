using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    // Library of movement that I ended up using in several places.
    // Note: I have no idea if it's a good way to organize code with C#+Unity, let's experiment?
    public class CircularMovement : IMoving
    {
        private IMoving baseMovement;

        private float angle = 0.0f;
        public  float radius = 5.0f;

        public CircularMovement(IMoving baseMovement, float radius)
        {
            this.baseMovement = baseMovement;
            this.radius = radius;
        }

        public void Move(Transform transform, float speed)
        {
            Move(transform, radius, speed);
        }

        public void Move(Transform transform, float radius, float speed)
        {
            baseMovement.Move(transform, speed);

            angle -= speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            transform.Translate( new Vector3(x, y, 0), Space.World);
        }
    }
}