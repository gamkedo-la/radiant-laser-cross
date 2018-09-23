using UnityEngine;
using System.Collections;

namespace rlc
{
    public class Bullet : MonoBehaviour
    {
        // TODO NOTE: here we are assuming that there is only one kind of bullet that just go forward
        // This code will have to be evolved to allow different kinds of bullets

        public float speed = 10.0f;
        public float lifetime_secs = 4.0f;
        private Vector3 direction;

        // Use this for initialization
        void Start()
        {
            Destroy(gameObject, lifetime_secs);
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(direction * (speed * Time.deltaTime), Space.World);
        }

        public void set_direction(Vector3 new_direction)
        {
            direction = new_direction.normalized;
        }
    }
}