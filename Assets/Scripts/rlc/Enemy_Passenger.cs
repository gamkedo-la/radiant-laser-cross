using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    /* Passenger: slowly pass-by while firing at slow pace.
     * */
    public class Enemy_Passenger : MonoBehaviour {

        public float speed = 4.0f;
        public float firing_interval = 3.0f;
        public Vector3 animation_rotation = new Vector3(1.0f, 1.0f, 0.0f);

        private float last_firing_timepoint;
        private GameObject body;

        // Use this for initialization
        void Start() {
            last_firing_timepoint = Time.time;

            body = GetComponentInChildren<Coloration>().gameObject;
        }

        // Update is called once per frame
        void Update() {
            Movement.move_forward(transform, speed);

            var now = Time.time;
            if (now - last_firing_timepoint > firing_interval)
            {
                last_firing_timepoint = now;
                var guns = GetComponentsInChildren<Gun>();
                foreach(var gun in guns)
                {
                    gun.fire(); // The orientation of guns have to be programmed separately
                }
            }

            body.transform.Rotate(animation_rotation);
        }
    }
}