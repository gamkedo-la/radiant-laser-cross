using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    public class Enemy : MonoBehaviour {

        public float speed = 4.0f;
        public float firing_interval = 3.0f;

        private IMoving movement;

        public Vector3 animation_rotation = new Vector3(1.0f, 1.0f, 0.0f);

        private float last_firing_timepoint;
        private GameObject body;
        private LifeControl life_control;

        // Use this for initialization
        void Start() {
            last_firing_timepoint = Time.time;
            body = GetComponentInChildren<ColoredBody>().gameObject;
            life_control = GetComponent<LifeControl>();
        }

        // Update is called once per frame
        void Update() {
            if (life_control.is_alive())
            {
                move();
                animate();
                maybe_fire();
            }
        }

        private void move()
        {
            Movement.Move(transform, speed);
        }

        private void animate()
        {
            body.transform.Rotate(animation_rotation); // small animation for the style
        }

        private void maybe_fire()
        {
            //if (is_visible) // TODO: make this work
            {
                var now = Time.time;
                if (now - last_firing_timepoint > firing_interval)
                {
                    last_firing_timepoint = now;
                    var guns = GetComponentsInChildren<Gun>();
                    foreach (var gun in guns)
                    {
                        gun.fire(); // The orientation of guns have to be programmed separately
                    }
                }
            }
        }

        public IMoving Movement
        {
            get
            {
                return movement;
            }

            set
            {
                movement = value;
            }
        }
    }
}