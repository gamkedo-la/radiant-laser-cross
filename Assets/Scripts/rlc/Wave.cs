using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace rlc
{

    /* Waves of ennemies.
     * This is the bigges brick of the level design.
     * Each Wave is a bunch of ennemies plus a graphic and audio theme.
     * This is designed so that wave's parametters could be changing through time.
     * USAGE:
     *  - Use this component for an object representing a wave, that can be put in the level sets;
     *  - Inherit from this class if you want the wave to have procedural behavior (driven by some code);
     */
    public class Wave : MonoBehaviour
    {
        public const string ENEMY_TAG = "enemy";

        // Background color that will be used while this wave is running.
        public Color background_color;

        // Title that will be displayed on the screen before starting the wave.
        public string title;

        public float timeout_secs = 120.0f;

        public UnityEvent on_finished;

        // TODO: Add audio tracks
        // TODO: Add background

        void Start()
        {

        }

        void Update()
        {

        }

        // Call this to spawn enemies procedurally.
        protected GameObject spawn_enemy(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            // All enemies must be part of the wave, to be able to clear them on game reset.
            var enemy = (GameObject)Instantiate(prefab, position, rotation, this.transform);
            // All enemies must be tagged as such.
            enemy.tag = ENEMY_TAG;
            return enemy;
        }

    }

}
