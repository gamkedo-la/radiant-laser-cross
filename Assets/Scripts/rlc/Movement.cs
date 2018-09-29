using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    // Library of "complex" movement logic to compose behaviors of bullets, ennemies, etc.
    static public class Movement
    {
        static public void move_forward(Transform transform, float speed)
        {
            transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);
        }

    }
}