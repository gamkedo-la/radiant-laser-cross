using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    /* Behaviour of any kind of shield, as in "protects from bullets with matching colors".
     * Shields are colored bodies, but usually they are not associated to a life point.
     * Therefore they act like a body part that does not have a life to kill, and can be activated or not.     *
     */
    public class Shield : ColoredBody
    {
        private enum State
        {
            inactive, active
        }
        private State state = State.inactive;

        void Start()
        {
            hide();
        }

        void Update()
        {
        }

        public void activate()
        {
            state = State.active;
            show();
        }

        public void deactivate()
        {
            if (state != State.active)
                return;
            state = State.inactive;
            hide();
        }

        // TODO: check if we need to keep the object active but still render or not depending on these functions.
        private void hide()
        {
            gameObject.SetActive(false);
        }

        private void show()
        {
            gameObject.SetActive(true);
        }

    }
}