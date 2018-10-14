using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace rlc
{
    /* Build and present levels.
     * Build levels as sequences of waves to play.
    */
    public class ProceduralLevelBuilder : MonoBehaviour
    {
        const string DEFAULT_TITLE = "RADIANT LASER CROSS";

        public List<Wave> waves_lvl_1_easy = new List<Wave>();
        public List<Wave> waves_lvl_2_challenging = new List<Wave>();
        public List<Wave> waves_lvl_3_hard = new List<Wave>();
        public List<Wave> waves_lvl_4_hardcore = new List<Wave>();

        public List<Wave> boss_lvl_1_challenging = new List<Wave>();
        public List<Wave> boss_lvl_2_hard = new List<Wave>();
        public List<Wave> boss_lvl_3_hardcore = new List<Wave>();

        public int end_level = 4;
        public int current_level_number = 1;
        public List<Wave> current_level_waves_selection;

        public UnityEngine.Object laser_cross_prefab;
        public Color default_background_color;

        public Text progress_display;
        public Text title_display;
        public float default_title_display_duration_secs = 5.0f;
        public float title_display_duration_secs = 3.0f;

        public enum State
        {
            ready, playing_wave, game_over
        }
        private State state = State.ready;
        private Wave current_wave;
        private IEnumerator<LevelStatus> level_progression;

        // Use this for initialization
        void Start()
        {
            reset_all();
        }

        // Update is called once per frame--*
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

            set_theme_color(default_background_color);

            level_progression = null;

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

            StartCoroutine(display_title("", DEFAULT_TITLE, default_title_display_duration_secs));

            state = State.ready;
        }

        public void new_game()
        {
            if (state == State.playing_wave)
                return;

            level_progression = make_level_progression();
            next_wave();
        }

        public void next_wave() // TODO: call this automatically when a wave is finished
        {
            if (level_progression == null)
                return;

            level_progression.MoveNext();
            if (level_progression.Current == LevelStatus.next_level)
            {
                // TODO: Do something to clarify that we changed level
                level_progression.MoveNext();
            }
        }

        private Wave pick_a_wave_in(IList<Wave> wave_bag)
        {
            if (wave_bag.Count == 0)
            {
                Debug.LogErrorFormat("No ennemies in enemy wave bag: {0}", wave_bag);
                return null;
            }
            var random_idx = Random.Range(0, wave_bag.Count);
            var picked_wave = wave_bag[random_idx];
            return picked_wave;
        }

        private void clear_wave()
        {
            if (current_wave != null) // TODO: remove the previous wave progressively/"smoothly"
            {
                Destroy(current_wave.gameObject);
            }
        }

        private IEnumerator start_wave(Wave wave, int level_idx, int wave_idx)
        {
            clear_wave();

            state = State.playing_wave;
            set_theme_color(wave.background_color);
            string progress_title = string.Format("Level {0} - Wave {1}", level_idx, wave_idx);
            yield return display_title(progress_title, wave.title, title_display_duration_secs);
            current_wave = Instantiate(wave);
        }

        private void set_theme_color(Color color)
        {
            // TODO: transition in a progressive way
            Camera.main.backgroundColor = color;
            RenderSettings.skybox.color = color;
            RenderSettings.skybox.SetColor("_Color", color);
            if (RenderSettings.skybox.HasProperty("_Tint"))
                RenderSettings.skybox.SetColor("_Tint", color);
            else if (RenderSettings.skybox.HasProperty("_SkyTint"))
                RenderSettings.skybox.SetColor("_SkyTint", color);
        }

        private IEnumerator display_title(string progress_text, string title_text, float duration_secs)
        {
            if (title_display == null || progress_display == null)
            {
                Debug.LogError("Incomplete text set to display the title!");
                yield break;
            }

            Debug.LogFormat("Progress: {0}", progress_text);
            Debug.LogFormat("Title: {0}", title_text);
            title_display.text = title_text;
            title_display.enabled = true;
            progress_display.text = progress_text;
            progress_display.enabled = true;

            yield return new WaitForSeconds(duration_secs);

            title_display.enabled = false;
            progress_display.enabled = false;
            Debug.Log("Title hidden");
        }


        private enum LevelStatus {
            next_wave,      // We'll play the next wave
            next_level,     // We'll play the next level
            finished        // End of level sequence reached! The player won!
        }

        private IEnumerator<LevelStatus> make_level_progression()
        {
            /* Notes: this is a coroutine (see usage of `yield` below).
             * It is called each time we need to progress through the list
             * of waves/levels. Each time `yield return <...>` is called,
             * it returns immediately the value but keeps track of where it
             * was in this function. Next time this function is caleld, it will
             * resume where it was.
             * */
            for (current_level_number = 1; current_level_number <= end_level; ++current_level_number)
            {
                current_level_waves_selection = build_level(current_level_number);
                yield return LevelStatus.next_level;

                int wave_idx = 0;
                foreach (Wave wave in current_level_waves_selection)
                {
                    ++wave_idx;
                    StartCoroutine(start_wave(wave, current_level_number, wave_idx));
                    yield return LevelStatus.next_wave;
                }
            }

            yield return LevelStatus.finished;
        }


        private List<Wave> build_level(int level_number)
        {
            if (level_number < 1)
            {
                Debug.LogErrorFormat("Wrong level number: {0}", level_number);
                return null;
            }

            List<Wave> selected_waves = new List<Wave>();

            switch (level_number)
            {
                case 1:
                    {
                        selected_waves.Add(pick_a_wave_in(waves_lvl_1_easy));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_1_easy));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_1_easy));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(boss_lvl_1_challenging));
                        break;
                    }
                case 2:
                    {
                        selected_waves.Add(pick_a_wave_in(waves_lvl_1_easy));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_1_easy));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(boss_lvl_2_hard));
                        break;
                    }
                case 3:
                    {
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                        selected_waves.Add(pick_a_wave_in(boss_lvl_2_hard));
                        break;
                    }
                case 4:
                    {
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                        selected_waves.Add(pick_a_wave_in(waves_lvl_1_easy));
                        selected_waves.Add(pick_a_wave_in(boss_lvl_1_challenging));
                        selected_waves.Add(pick_a_wave_in(boss_lvl_2_hard));
                        selected_waves.Add(pick_a_wave_in(boss_lvl_2_hard));
                        selected_waves.Add(pick_a_wave_in(boss_lvl_3_hardcore));
                        break;
                    }
                default:
                    {
                        // TODO: for an "infinite mode", just put some kind of algorithm here.
                        return null;
                    }

            }

            return selected_waves;
        }

    }

}
