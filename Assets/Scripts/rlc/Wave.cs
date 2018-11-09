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

        public enum State
        {
            SettingUp,      // The wave is being set-up, do not finish it even if there is no enemies left.
            Ready,      // Ready to finish whenever the enemies are all left from the screen.
            Finished    // This wave finished, nothing else will happen.
        }

        private State state = State.Ready; // TODO: make it visible but read-only

        [Tooltip("Background color that will be used while this wave is running.")]
        public Color background_color;

        [Tooltip("Title that will be displayed on the screen before starting the wave.")]
        public string title;

        [Tooltip("Timeout, if reached it's game over. Zero means no timeout.")]
        public float timeout_secs = 0.0f;

        public delegate void WaveEvent(Wave wave);

        [Tooltip("Called once this wave is finished.")]
        public WaveEvent on_finished;

        [Tooltip("Count of enemies left before this wave finishes.")]
        public int enemies_left_count = 0;

        // TODO: Add audio tracks
        // TODO: Add background

        void Start()
        {
            var all_preset_enemies = GameObject.FindGameObjectsWithTag(ENEMY_TAG);
            foreach(var enemy in all_preset_enemies)
            {
                prepare_enemy(enemy);
            }
        }

        void Update()
        {

        }

        // Make this wave in the SettingUp state, preventing it to finish even if there is no enemies left.
        protected void setting_up()
        {
            state = State.SettingUp;
        }

        // Make this wave in the Ready state, allowing it to finish even if there is no enemies left.
        protected void ready()
        {
            state = State.Ready;
            check_end_of_wave();
        }

        // Call this to spawn enemies procedurally, setting them so that we can track them.
        protected GameObject spawn_enemy(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            // All enemies must be part of the wave, to be able to clear them on game reset.
            var enemy = (GameObject)Instantiate(prefab, position, rotation, this.transform);
            prepare_enemy(enemy);
            return enemy;
        }

        private void prepare_enemy(GameObject enemy)
        {
            // All enemies must be tagged as such.
            enemy.tag = ENEMY_TAG;

            // Keep track of enemies count
            var enemy_life = enemy.GetComponent<LifeControl>();
            enemy_life.on_destroyed += this.on_one_enemy_destroyed;

            ++enemies_left_count;
        }

        private void on_one_enemy_destroyed(LifeControl enemy_life)
        {
            --enemies_left_count;
            if (enemies_left_count < 0)
            {
                Debug.LogError("Enemy tracking logic in Wave failed");
                enemies_left_count = 0;
            }

            check_end_of_wave();
        }

        private void check_end_of_wave()
        {
            if (state == State.Ready && enemies_left_count == 0)
            {
                state = State.Finished;
                notify_finished();
            }
        }

        private void notify_finished()
        {
            if (on_finished != null)
                on_finished(this);
        }
    }

}
