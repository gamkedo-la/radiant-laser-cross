using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    // This is an interface that serves as a basis for the implementation
    // of a the Decorator Pattern to all moving objects in RLC.
    public interface IMoving
    {
        void Move(Transform transform, float speed);
    }
}