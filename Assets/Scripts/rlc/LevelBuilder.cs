using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace rlc
{
    /* Build and present levels.
     * Build levels as sequences of waves to play.
    */
    public class LevelBuilder : MonoBehaviour
    {
        public List<Wave> waves_difficulty_0 = new List<Wave>();
        public List<Wave> waves_difficulty_1 = new List<Wave>();
        public List<Wave> waves_difficulty_2 = new List<Wave>();

        public float timeout_in_secs = 120.0f;

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
