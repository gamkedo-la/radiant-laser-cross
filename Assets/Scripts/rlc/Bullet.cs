using UnityEngine;
using System.Collections;

namespace rlc
{
    public class Bullet : MonoBehaviour
    {
        public const string TAG = "bullet";

        public float speed = 10.0f;
        public float acceleration = 0.0f;

        // Clan who fired the bullet (imutable once set)
        private Clan clan_who_fired;

        // Clan who own the bullet (it can change if the opposite clan reflects it)
        public Clan clan_who_owns = Clan.enemy;

        private Movable movable;
        private ColoredBody my_body;
        private bool is_reflected = false;
        private bool has_hit_body = false;

        void Start()
        {
            movable = GetComponentInParent<Movable>();
            my_body = GetComponent<ColoredBody>();
            if (my_body == null)
            {
                Debug.LogError("Bullet objects must have a ColoredBody component!");
            }

        }

        void FixedUpdate()
        {
            speed += acceleration;
            movable.MoveForward(speed);
        }


        public static void clear_bullets_from_game()
        {
            var bullets = GameObject.FindGameObjectsWithTag(Bullet.TAG);
            foreach (var bullet in bullets)
            {
                Destroy(bullet);
            }
        }
        public bool is_player_responsability()
        {
            return this.ClanWhoFired == Clan.player && this.clan_who_owns == Clan.player;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var body_hit = collision.collider.GetComponentInParent<ColoredBody>();
            var bullet_hit = collision.collider.GetComponentInParent<Bullet>();
            if (body_hit != null    // hit a colored body...
            && bullet_hit == null   // ... which is not another bullet...
            )
            {
                // Debug.Log("OnCollisionEnter" + name + " and " + collision.gameObject.name);

                bool colors_matches = ColorSystem.colors_matches(body_hit.color_family, my_body.color_family);
                bool hitting_the_enemy = clan_who_owns != body_hit.clan; // ... we are either enemy bullet hitting player or the reverse...
                bool on_hit_invoked = false;
                if(colors_matches) {
                    if (hitting_the_enemy)
                    {
                        // ... We hit an enemy matching the right color!
                        BulletEvents.InvokeOnHit(this, body_hit);
                        on_hit_invoked = true;
                        body_hit.on_hit();
                    }
                    if (body_hit.surface_effect == ColoredBody.SurfaceEffect.reflective)
                        end_with_reflection(collision, body_hit);
                }

                // We hit something solid, so the bullet will end anyway.
                if (body_hit.surface_effect == ColoredBody.SurfaceEffect.solid)
                    end_with_impact(collision);

                if(!on_hit_invoked)
                {
                    BulletEvents.InvokeOnAbsorved(this, body_hit);
                }
                has_hit_body = true;
            }

        }

        private void end_with_reflection(Collision collision, ColoredBody body_hit)
        {
            var bullet_velocity = movable.Velocity;
            var body_velocity = body_hit.Movable.Velocity;
            if (!is_reflected)
            { // To avoid multiple reflective collisions
                is_reflected = true;
                play_impact_animation(); // TODO: replace by another impact?

                // Now for the rest of the lifetime, we just go in another direction\
                var body_influence_ratio = 2f; // The body movement influences to the reflected direction
                var impact_direction = (bullet_velocity - body_velocity * body_influence_ratio).normalized;
                transform.forward = Vector3.Reflect(impact_direction, collision.contacts[0].normal);

                // As soon as the bullet is reflected, it can hit anybody matching it!
                clan_who_owns = body_hit.clan;
            }

            // Push the bullet againt the shield
            var min_bullet_push = 1f; // Minimum distance the bullet will be pushed (to be safer from glitches)
            var translation = transform.forward * Mathf.Max(min_bullet_push, -collision.contacts[0].separation + body_hit.Movable.LastMove.magnitude + movable.LastMove.magnitude);
            if (Mathf.Abs(translation.z) >= 0.1f)
            {
                Debug.LogError("WRONG Z REFLECTION");
            }
            transform.Translate(translation, Space.World);
        }


        private void end_with_impact(Collision collision)
        {
            play_impact_animation();
            Destroy(gameObject);
        }

        private void play_impact_animation()
        {
            // TODO: add impact animation here
        }

        private void OnBecameInvisible()
        {
            // Destroy any bullet getting out of the screen.
            // This is not only to avoid keeping them alive for nothning,
            // it also avoids enemies not yet entering the screen to ever be hit
            // by lost bullets.
            if(!has_hit_body) BulletEvents.InvokeOnMiss(this);
            Destroy(gameObject);
        }

        public Clan ClanWhoFired {
            get { return this.clan_who_fired; }
            set {
                this.clan_who_fired = value;
                this.clan_who_owns = value;
            }
        }
    }
}