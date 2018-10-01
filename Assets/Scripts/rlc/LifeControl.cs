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

        // Use this for initialization
        void Start()
        {
            life_state = LifeState.alive;
            if (body_parts.Count == 0)
            {
                Debug.LogErrorFormat("Destructible object {} have no body parts set!", this.name);
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
            // TODO: REMOVE ME
            if (Input.GetKeyDown(KeyCode.Delete))
                die();
        }

        public bool is_alive()
        {
            return life_state == LifeState.alive;
        }

        public void on_hit()
        {
            if (!is_alive())
                return;

            --hit_points;
            if (hit_points == 0)
            {
                die();
            }
            else
            {
                // TODO: play "hit" animation here
            }
        }

        public void die()
        {
            life_state = LifeState.dying;
            Destroy(gameObject, 1.0f);
        }

    }
}