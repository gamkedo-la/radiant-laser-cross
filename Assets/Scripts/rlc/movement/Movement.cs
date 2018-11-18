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
            move_direction(transform, transform.forward, speed);
        }

        static public void move_direction(Transform transform, Vector3 direction, float speed)
        {
            transform.Translate(direction * (speed * Time.deltaTime), Space.World);

            // Belows the attempt to use physiucs to detect continuous collisions.
            //var translation = direction.normalized * speed /** Time.deltaTime*/;
            //var rigid_body = transform.GetComponentInChildren<Rigidbody>();
            //if (rigid_body)
            //{
            //    rigid_body.velocity = translation;
            //    //rigid_body.AddForce(translation, ForceMode.VelocityChange);
            //    Debug.LogFormat("MOVE RLC: {0}", translation);
            //}
            //else
            //{
            // do the normal thing
            //}
        }

    }
}