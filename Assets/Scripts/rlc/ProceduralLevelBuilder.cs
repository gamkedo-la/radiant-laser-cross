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

        private bool is_running = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void new_game()
        {
            if (is_running)
                return;

            if (waves_difficulty_1_tutoring.Count == 0)
            {
                Debug.LogError("No wave to play!");
                return;
            }

            is_running = true;

            var picked_wave = pick_a_wave_in(waves_difficulty_1_tutoring);
            Instantiate(picked_wave);
        }

        private Wave pick_a_wave_in(IList<Wave> wave_bag)
        {
            var random_idx = Mathf.CeilToInt(Random.Range(0, wave_bag.Count - 1));
            var picked_wave = wave_bag[random_idx];
            return picked_wave;
        }
    }

}
