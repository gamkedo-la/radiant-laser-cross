using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace rlc
{
    /** For things that have life and can be destroyed/killed (can take hits etc.)
     * */
    public class LifeControl : MonoBehaviour
    {

        private enum LifeState
        {
            alive_newborn,      // Alive but not entered screen yet.
            alive_in_screen,    // Alive and in screen.
            dying               // Dying (doing it's dying animation).
        }
        private LifeState life_state = LifeState.alive_newborn; // TODO: read-only public access

        public int hit_points = 1;
        public List<ColoredBody> body_parts = new List<ColoredBody>();

        public delegate void LifeEvent(LifeControl life);
        public LifeEvent on_destroyed;


        public GameObject explosionPrefab;
        public GameObject hitPrefab;

        // Use this for initialization
        void Start()
        {
            if (body_parts.Count == 0)
            {
                Debug.LogErrorFormat("Destructible object {0} have no body parts set!", this.name);
            }

            foreach(var body in body_parts)
            {
                if (body.life_control != null)
                {
                    Debug.LogError("Body associated with more than one body part", this);
                }
                body.life_control = this;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public bool is_alive()
        {
            return life_state != LifeState.dying;
        }

        public bool is_alive_in_screen()
        {
            return life_state == LifeState.alive_in_screen;
        }

        public bool is_newborn()
        {
            return life_state == LifeState.alive_newborn;
        }

        public void on_hit() // TODO: add the position of the hit point
        {
            //Debug.Log("on_hit!");

            if (!is_alive_in_screen())
                return;

            --hit_points;
            if (hit_points == 0)
            {
                //Debug.Log("on_hit: die!");
                ExplodeFX();
                die();
            }
            else
            {
                HitFX();
                // TODO: play "hit" animation here
            }
        }

        public void die()
        {
            life_state = LifeState.dying;
            Destroy(gameObject, 1.0f);
        }

        public void on_entered_screen()
        {
            life_state = LifeState.alive_in_screen;
        }

        public void ExplodeFX()
        {
            if (explosionPrefab) // was a prefab set in inspector?
            {
                GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(explosion, 5);
            }
        }

        public void HitFX()
        {
            if (hitPrefab)
            {
                GameObject explosion = Instantiate(hitPrefab, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(explosion, 5);
            }
        }

        private void OnDestroy()
        {
            notify_destroyed();
        }

        private void notify_destroyed()
        {
            if (on_destroyed != null)
                on_destroyed(this);
        }

    }
}