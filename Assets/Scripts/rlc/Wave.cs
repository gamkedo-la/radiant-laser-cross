using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

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
        public const float ENEMY_SPAWN_WARNING_DELAY = 2.0f;

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


        static private Material warning_material = null;

        void Start()
        {
            if (warning_material == null)
            {
                warning_material = (Material)Resources.Load("warning_color", typeof(Material));
            }

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
            // TODO: if we spawn the enemy in the screen, add a default visual warning.
            return spawn_enemy_with_warning(prefab, position, rotation, ENEMY_SPAWN_WARNING_DELAY);
        }



        private GameObject spawn_enemy_now(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            // All enemies must be part of the wave, to be able to clear them on game reset.
            var enemy = (GameObject)Instantiate(prefab, position, rotation, this.transform);
            prepare_enemy(enemy);
            return enemy;
        }

        // Call this to spawn enemies with a visual warning, setting them immediately so that we can track them.
        private GameObject spawn_enemy_with_warning(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
        {
            var new_enemy = spawn_enemy_now(prefab, position, rotation);
            StartCoroutine(start_spawn_warning(new_enemy, delay));
            return new_enemy;
        }

        static private IEnumerator start_spawn_warning(GameObject target, float delay)
        {
            target.SetActive(false);
            var warning_list = new List<GameObject>();
            foreach (Collider collider in target.GetComponentsInChildren<Collider>())
            {
                warning_list.Add(spawn_warning_fx(collider));
            }

            yield return new WaitForSeconds(delay);

            foreach (var warning_object in warning_list)
            {
                const float fx_end_delay_after_actual_spawn = 1.0f;
                Destroy(warning_object, fx_end_delay_after_actual_spawn);
            }

            target.SetActive(true);
        }

        static private GameObject spawn_warning_fx(Collider collider)
        {
            var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.position = collider.transform.position;
            box.transform.rotation = collider.transform.rotation;

            box.transform.localScale = collider.GetComponent<Renderer>().bounds.size;

            var box_renderer = box.GetComponentInChildren<Renderer>();
            box_renderer.material = warning_material;

            return box;
        }

        private void prepare_enemy(GameObject enemy)
        {
            // All enemies must be tagged as such.
            enemy.tag = ENEMY_TAG;

            // Keep track of enemies count
            var enemy_life = enemy.GetComponent<LifeControl>();
            enemy_life.on_destroyed += this.on_one_enemy_destroyed;

            ++enemies_left_count;

            // Force up to the direction to the camera.
            enemy.transform.rotation = Quaternion.LookRotation(enemy.transform.forward, Vector3.back);
            // Force position ont the game's plan.
            enemy.transform.position.Set(enemy.transform.position.x, enemy.transform.position.y, 0);
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
