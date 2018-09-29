using UnityEngine;
using System.Collections;

namespace rlc
{
    public class Lifetime : MonoBehaviour
    {

        public float lifetime_secs = 4.0f; // How many seconds after spawning will it disappear.

        // Use this for initialization
        void Start()
        {
            Destroy(gameObject, lifetime_secs);
        }

    }
}