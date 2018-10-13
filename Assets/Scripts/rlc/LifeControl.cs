using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace rlc
{
    /** For things that have life and can be destroyed/killed (can take hits etc.)
     * */
    public class LifeControl : MonoBehaviour
    {

        private enum LifeState
        {
            none, alive, dying
        }
        private LifeState life_state = LifeState.none;

        public int hit_points = 1;
        public List<ColoredBody> body_parts = new List<ColoredBody>();
        public GameObject explosionPrefab;
        public GameObject hitPrefab;

        // Use this for initialization
        void Start()
        {
            life_state = LifeState.alive;
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
            return life_state == LifeState.alive;
        }

        public void on_hit() // TODO: add the position of the hit point
        {
            //Debug.Log("on_hit!");

            if (!is_alive())
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

        public void die()
        {
            life_state = LifeState.dying;
            Destroy(gameObject, 1.0f);
        }

    }
}