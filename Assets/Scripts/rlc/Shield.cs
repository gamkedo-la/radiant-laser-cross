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
        private AudioSource shield_sound;

        new void Start()
        {
            shield_sound = GetComponent<AudioSource>();
            hide();
            base.Start();
        }

        void Update()
        {
            if (state == State.inactive)
                hide();
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
            // Note that the actual change of state is defered to the Update().
            // This is because we could be deactivated and reactivated every frame.
            // We assume that Update() will be called once, instead of this function being called
            // several time every frame.
        }

        // TODO: check if we need to keep the object active but still render or not depending on these functions.
        private void hide()
        {
            gameObject.SetActive(false);
            stop_sound();
        }

        private void show()
        {
            gameObject.SetActive(true);
            start_sound();
        }

        private void start_sound()
        {
            if (shield_sound && !shield_sound.isPlaying)
            {
                Debug.LogFormat("Play shield {0} sound", gameObject.name);
                shield_sound.Play();
            }
        }

        private void stop_sound()
        {
            if (shield_sound)
            {
                Debug.LogFormat("Stop shield {0} sound", gameObject.name);
                shield_sound.Stop();
            }
        }

    }
}