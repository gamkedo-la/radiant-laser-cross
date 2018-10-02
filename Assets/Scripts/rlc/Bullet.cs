using UnityEngine;
using System.Collections;

namespace rlc
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10.0f;
        public bool is_from_player = false;
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
            var body_hit = collision.collider.GetComponent<ColoredBody>();
            var bullet_hit = collision.collider.GetComponent<Bullet>();
            if (body_hit != null    // hit a colored body...
            && bullet_hit == null   // ... which is not another bullet...
            )
            {
                Debug.Log("OnCollisionEnter" + name + " and " + collision.gameObject.name);

                // We hit something solid, so the bullet will end anyway.
                play_impact_animation();

                if (is_from_player != body_hit.is_player // ... we are either enemy bullet hitting player or the reverse...
                && ColorSystem.colors_matches(body_hit.color_family, my_body.color_family)
                )
                {
                    // ... We hit something matching the right color!
                    body_hit.life_control.on_hit();
                }
            }

        }

        private void play_impact_animation()
        {
            // TODO: add impact animation here and plan for destruction after it.
            Destroy(gameObject);
        }

    }
}