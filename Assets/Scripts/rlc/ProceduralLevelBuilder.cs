using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace rlc
{
    /* Build and present levels.
     * Build levels as sequences of waves to play.
    */
    public class ProceduralLevelBuilder : MonoBehaviour
    {
        public List<Wave> waves_difficulty_1_tutoring           = new List<Wave>();
        public List<Wave> waves_difficulty_2_easy               = new List<Wave>();
        public List<Wave> waves_difficulty_3_challenging        = new List<Wave>();
        public List<Wave> waves_difficulty_4_hard               = new List<Wave>();

        public List<Wave> boss_waves_difficulty_1_easy          = new List<Wave>();


        public List<Wave> boss_waves_difficulty_2_challenging   = new List<Wave>();
        public List<Wave> boss_waves_difficulty_3_hard          = new List<Wave>();

        public UnityEngine.Object laser_cross_prefab;
        public Color default_background_color;

        public enum State
        {
            ready, playing_wave, game_over
        }
        private State state = State.ready;
        private Wave current_wave;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (state == State.game_over)
            {
                reset_all();
            }
        }


        public void game_over()
        {
            if (state != State.playing_wave)
                return;
            state = State.game_over;
        }

        private void reset_all()
        {
            Debug.Log("==== RESET ALL ====");
            // TODO: make a not crude version XD

            if (current_wave != null)
            {
                Destroy(current_wave.gameObject);
            }

            Camera.main.backgroundColor = default_background_color;

            var laser_cross = GameObject.Find("laser_cross");
            if (laser_cross == null || !laser_cross.GetComponent<LaserCross>().life_control.is_alive())
            {
                if (laser_cross != null)
                {
                    Destroy(laser_cross.gameObject);
                }

                laser_cross = (GameObject)GameObject.Instantiate(laser_cross_prefab, Vector3.zero, Quaternion.identity);
                laser_cross.name = "laser_cross";
            }

            state = State.ready;
        }

        public void new_game()
        {
            if (state == State.playing_wave)
                return;

            if (waves_difficulty_1_tutoring.Count == 0)
            {
                Debug.LogError("No wave to play!");
                return;
            }

            state = State.playing_wave;

            var picked_wave = pick_a_wave_in(waves_difficulty_1_tutoring);
            current_wave = Instantiate(picked_wave);

            Camera.main.backgroundColor = picked_wave.background_color; // TODO: transition in a progressive way
        }

        private Wave pick_a_wave_in(IList<Wave> wave_bag)
        {
            if (wave_bag.Count == 0)
            {
                Debug.LogErrorFormat("No ennemies in enemy wave bag: {}", wave_bag);
                return null;
            }
            var random_idx = Random.Range(0, wave_bag.Count);
            var picked_wave = wave_bag[random_idx];
            return picked_wave;
        }
    }

}
