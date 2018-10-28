using UnityEngine;
using System.Collections;

namespace rlc
{

    public class GameUpdater : MonoBehaviour
    {

        public GameCamera game_camera;
        public float seconds_between_rules_update = 1.0f;
        public float deadline_to_enter_the_screen = 20.0f;

        // Use this for initialization
        void Start()
        {
            if (game_camera == null)
            {
                Debug.LogError("No game camera set in GameUpdater!");
            }

            StartCoroutine(slow_update());
        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator slow_update()
        {
            while (true)
            {
                remove_enemies_out_of_screen();
                yield return new WaitForSeconds(seconds_between_rules_update);
            }
        }

        private void remove_enemies_out_of_screen()
        {
            //Debug.Log("Removing enemies that got out of the screen...");
            var all_enemies = GameObject.FindGameObjectsWithTag(Wave.ENEMY_TAG);
            foreach (var enemy in all_enemies)
            {
                var life_points = enemy.GetComponentsInChildren<LifeControl>();
                if (game_camera.is_able_to_see(enemy))
                {
                    foreach (var life in life_points)
                    {
                        if (life.is_newborn())
                        {
                            life.on_entered_screen();
                        }
                    }
                }
                else
                {
                    foreach (var life in life_points)
                    {
                        if (life.is_alive_in_screen())
                        {
                            Destroy(enemy);
                            break; // No need to update everything.
                        }
                        else
                        {
                            if (life.is_newborn())
                            {
                                // Destroy enemies that never enter the screen after some time.
                                var time_passed_since_creation = Time.time - life.start_time;
                                if (time_passed_since_creation > deadline_to_enter_the_screen)
                                {
                                    Debug.Log("Destroying Newborn enemy that didn't enter in time in the screen.");
                                    Destroy(enemy);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //Debug.Log("Removing enemies that got out of the screen - DONE");
        }
    }
}