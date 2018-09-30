using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    // Library of movement that I ended up using in several places.
    // Note: I have no idea if it's a good way to organize code with C#+Unity, let's experiment?
    static public class Movement
    {
        static public void move_forward(Transform transform, float speed)
        {
            transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);
        }

    }
}