using UnityEngine;
using System.Collections;

namespace rlc
{

    public class OverloadSystem : MonoBehaviour
    {
        public float load_limit = 10.0f;

        public float load = 0.0f;
        private float next_update_load = 0.0f;

        public float load_speed = 1.0f;
        public float load_recovery_speed = 1.0f;
        public float overload_cost = 5.0f;

        public enum State
        {
            stable,     // load is growing lower than restoration and we are under overload
            heating,    // load is growing higher than restoration and we are under overload
            overload,   // load is over overload limit
        }
        public State state = State.stable;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var new_load = load;

            new_load += next_update_load * load_speed * Time.deltaTime;
            new_load -= load_recovery_speed * Time.deltaTime;

            if (new_load < 0)
            {
                new_load = 0;
                if (state == State.overload)
                    state = State.stable;
            }

            if (state != State.overload)
            {
                if (new_load > load_limit)
                {
                    state = State.overload;
                    new_load += overload_cost;
                }
                else
                if (new_load > load)
                {
                    state = State.heating;
                }
                else
                {
                    state = State.stable;
                }
            }

            load = new_load;

            next_update_load = 0;
        }


        public void add_load()
        {
            if (state == State.overload)
                return;

            next_update_load += 1;
        }



    }
}