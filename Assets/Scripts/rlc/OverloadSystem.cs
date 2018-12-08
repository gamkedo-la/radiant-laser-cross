﻿using UnityEngine;
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
            overloading,    // load is growing higher than restoration and we are under overload
            recovering,   // load is over overload limit
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
                if (state == State.recovering)
                {
                    state = State.stable;
                    OverloadEvents.InvokeOnUnblock();
                }
            }

            if (state != State.recovering)
            {
                if (new_load > load_limit)
                {
                    state = State.recovering;
                    new_load += overload_cost;
                }
                else
                if (new_load > load)
                {
                    state = State.overloading;
                }
                else
                {
                    state = State.stable;
                }
            }

            load = new_load;

            next_update_load = 0;
            OverloadEvents.InvokeOnLoadChange(load);

            if (state == State.recovering)
            {
                var recover_percent = 1f - load / (load_limit + overload_cost);
                recover_percent = Mathf.Max(0f, recover_percent);
                recover_percent = Mathf.Min(1f, recover_percent);
                OverloadEvents.InvokeOnBlock(recover_percent);
            }
        }

        public void add_load(float load)
        {
            if (state == State.recovering)
                return;

            next_update_load += load;
        }



    }
}