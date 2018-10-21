using UnityEngine;

namespace rlc
{
    // Library of movement that I ended up using in several places.
    // Note: I have no idea if it's a good way to organize code with C#+Unity, let's experiment?
    sealed public class LinearMovement : IMoving
    {
        public void Move(Transform transform, float speed)
        {
            transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);
        }
    }
}