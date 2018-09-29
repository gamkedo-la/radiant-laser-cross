using UnityEngine;
using System.Collections;

namespace rlc
{

    /* Waves of ennemies.
     * This is the bigges brick of the level design.
     * Each Wave is a bunch of ennemies plus a graphic and audio theme.
     * This is designed so that wave's parametters could be changing through time.
     */
    public class Wave : MonoBehaviour
    {
        // Background color that will be used while this wave is running.
        public Color background_color;

        // Title that will be displayed on the screen before starting the wave.
        public string title;

        public float timeout_secs = 120.0f;


        // TODO: Add audio tracks
        // TODO: Add background

        void Start()
        {

        }

        void Update()
        {

        }

        public void launch()
        {

        }

    }

}
