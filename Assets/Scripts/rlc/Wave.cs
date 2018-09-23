using UnityEngine;
using System.Collections;

namespace rlc
{

    /* Waves of ennemies.
     * This is the bigges brick of the level design.
     * Waves can be defined by any class that implements this interface.
     * This is designed so that wave's parametters could be changing through time.
     */
    public interface Wave
    {
        // Background color that will be used while this wave is running.
        Color background_color();

        // Title that will be displayed on the screen before starting the wave.
        string title();

        // Called when the wave must begin.
        void on_entry();


    }

}
