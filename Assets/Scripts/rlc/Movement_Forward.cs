using UnityEngine;
using System.Collections;

namespace rlc
{
    public class Movement_Forward : MonoBehaviour
    {
        public float speed = 10.0f;

        void Update()
        {
            transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);
        }

    }
}