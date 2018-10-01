using UnityEngine;
using System.Collections;

namespace rlc
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10.0f;
        private ColoredBody my_body;

        void Update()
        {
            Movement.move_forward(transform, speed);
            my_body = GetComponent<ColoredBody>();
            if (my_body == null)
            {
                Debug.LogError("Bullet objects must have a ColoredBody component!");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("OnCollisionEnter" + name + " and " + collision.gameObject.name);
            var body_hit = collision.collider.GetComponent<ColoredBody>();
            var bullet_hit = collision.collider.GetComponent<Bullet>();
            if (body_hit != null    // hit a colored body...
            && bullet_hit == null   // ... which is not another bullet...
            && ColorSystem.colors_matches(body_hit.color_family, my_body.color_family))
            {
                // We hit something with the right color!
                body_hit.life_control.on_hit();
                play_impact_animation();
            }

        }

        private void play_impact_animation()
        {
            // TODO: add impact animation here and plan for destruction after it.
            Destroy(gameObject);
        }

    }
}