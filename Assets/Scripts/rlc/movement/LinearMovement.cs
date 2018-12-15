using UnityEngine;

namespace rlc
{
    sealed public class LinearMovement : IMoving
    {
        public void Move(Transform transform, float speed)
        {
            transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);
        }
    }
}