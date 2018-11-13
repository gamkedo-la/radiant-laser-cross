using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    public class TimeoutSystem : MonoBehaviour
    {
        public UI_Timeout display;


        public delegate void OnTimeout();

        public OnTimeout on_timeout;

        private IEnumerator current_run;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void prepare_timeout(float seconds)
        {
            display.show(seconds);
        }

        public void start(float seconds)
        {
            end_run();
            current_run = run_timeout(seconds);
            StartCoroutine(current_run);
        }

        public void stop()
        {
            display.hide();
            end_run();
        }

        private void end_run()
        {
            if (current_run != null)
            {
                StopCoroutine(current_run);
            }
        }

        private IEnumerator run_timeout(float timeout)
        {
            const float time_between_updates = 1.0f / 8.0f;

            float start_time = Time.time;
            float timeout_time = start_time + timeout;
            do
            {
                var now = Time.time;
                var time_left = timeout_time - now;

                display.show(time_left);

                if (time_left < 0)
                {
                    // HERE: timeout!
                    current_run = null;
                    on_timeout();
                    break;
                }

                yield return new WaitForSeconds(time_between_updates);
            }
            while (true);
        }

    }


}
