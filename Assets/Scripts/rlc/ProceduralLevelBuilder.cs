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


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
