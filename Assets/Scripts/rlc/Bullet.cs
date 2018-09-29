using UnityEngine;
using System.Collections;

namespace rlc
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10.0f;

        void Update()
        {
            Movement.move_forward(transform, speed);
        }

    }
}