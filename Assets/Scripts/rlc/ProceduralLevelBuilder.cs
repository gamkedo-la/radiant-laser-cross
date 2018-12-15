using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace rlc
{

    public enum GameOverReason
    {
        destroyed, timeout, hard_reset, exit
    }

    /* Build and present levels.
     * Build levels as sequences of waves to play.
    */
    public class ProceduralLevelBuilder : MonoBehaviour
    {
        const string DEFAULT_TITLE = "RADIANT LASER CROSS";


        public List<Wave> level_1_waves_1 = new List<Wave>();
        public List<Wave> level_1_waves_2 = new List<Wave>();
        public List<Wave> level_1_waves_3 = new List<Wave>();
        public List<Wave> level_1_waves_4 = new List<Wave>();
        public List<Wave> level_1_waves_5 = new List<Wave>();
        public List<Wave> level_1_waves_6 = new List<Wave>();
        public List<Wave> level_1_boss    = new List<Wave>();



        public int end_level = 4;
        private int current_level_number = 1;
        private int current_wave_number = 1;
        private List<WaveInfo> current_level_waves_selection;

        public UnityEngine.Object laser_cross_prefab;
        public Color default_background_color;
        public Background default_background_prefab;
        public float color_change_speed = 0.5f;
        public MusicTrack default_music_tracks;

        public List<GameObject> game_complete_splosions_prefabs;

        public UI_FancyText progress_display;
        public UI_FancyText title_display;
        public float default_title_display_duration_secs = 5.0f;
        public float title_display_duration_secs = 3.0f;

        public Text instruction_text;

        private TimeoutSystem timeout;
        private IEnumerator timeout_gameover_display;
        private float timeout_gameover_deplay = 3.0f;

        private ScoringSystem scoring;

        private int title_display_count = 0;
        private int wave_start_count = 0;

        public enum State
        {
            ready, playing_wave, game_over, exiting
        }
        private State state = State.ready;
        private Wave current_wave;
        private IEnumerator start_wave_coroutine;
        private IEnumerator<LevelStatus> level_progression;
        private Background current_background;
        private IEnumerator theme_color_progression;


        private enum WaveCategory
        {
            Wave, Boss
        }

        private class WaveInfo
        {
            public Wave wave;
            public WaveCategory category;
        }

        // Use this for initialization
        void Start()
        {
            timeout = GetComponent<TimeoutSystem>();
            scoring = GetComponent<ScoringSystem>();
            timeout.on_timeout = () => game_over_timeout();
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


        public void game_over(GameOverReason reason)
        {
            if (state == State.exiting)
                return;

            if (state != State.playing_wave)
                return;

            state = State.game_over;

            if (reason == GameOverReason.timeout)
            {
                if (timeout_gameover_display != null)
                    StopCoroutine(timeout_gameover_display);
                timeout_gameover_display = game_over_timeout_display();
                StartCoroutine(timeout_gameover_display);
            }
            else
            {
                timeout.stop();
            }
        }

        public void exit() // This is a special exit, only run when we want to get back to the main men
        {
            if (state == State.exiting)
                return;

            state = State.exiting;

            timeout.stop();

            display_game_title();

            if (LaserCross.current)
            {
                LaserCross.current.die(GameOverReason.exit);
            }

            var all_enemies = GameObject.FindGameObjectsWithTag(Wave.ENEMY_TAG);
            foreach (var enemy in all_enemies)
            {
                var life_controls = enemy.GetComponentsInChildren<LifeControl>();
                foreach (var life_control in life_controls)
                {
                    life_control.die();
                }
            }

            StartCoroutine(wait_then_exit());
        }

        private IEnumerator wait_then_exit()
        {
            yield return new WaitForSeconds(1.0f);
            clear_wave();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        private void game_over_timeout()
        {
            if (LaserCross.current)
            {
                LaserCross.current.die(GameOverReason.timeout);
            }
        }

        private IEnumerator game_over_timeout_display()
        {
            timeout.show_timeout(0.0f);
            yield return new WaitForSeconds(timeout_gameover_deplay);
            timeout.hide_timeout();
        }

        private void reset_all()
        {
            Debug.Log("==== RESET ALL ====");

            clear_wave();

            if (start_wave_coroutine != null)
            {
                StopCoroutine(start_wave_coroutine);
                start_wave_coroutine = null;
            }

            Bullet.clear_bullets_from_game();

            set_theme_color(default_background_color);
            launch_background(default_background_prefab);

            level_progression = null;

            const string LASER_CROSS_OBJECT_NAME = "laser_cross";

            GameObject laser_cross = GameObject.Find(LASER_CROSS_OBJECT_NAME);

            if (laser_cross == null || !laser_cross.GetComponent<LaserCross>().life_control.is_alive())
            {
                if (laser_cross != null)
                {
                    Destroy(laser_cross);
                }

                laser_cross = (GameObject)GameObject.Instantiate(laser_cross_prefab, Vector3.zero, Quaternion.identity);
                laser_cross.name = LASER_CROSS_OBJECT_NAME;
            }

            display_game_title();

            if (instruction_text)
            {
                instruction_text.enabled = true;
            }

            if (default_music_tracks)
            {
                MusicEventManager.Instance.Transition(default_music_tracks);
            }

            state = State.ready;
        }

        public void new_game()
        {
            if (state != State.ready)
                return;

            if (instruction_text)
            {
                instruction_text.enabled = false;
            }

            scoring.reset();
            UI_Scoring.Display(true);

            level_progression = make_level_progression();
            next_wave();
        }

        public void next_wave()
        {
            if (state != State.ready && state != State.playing_wave)
                return;

            if (level_progression == null)
                return;

            level_progression.MoveNext();

            switch (level_progression.Current)
            {
                case LevelStatus.next_level:
                {
                    level_progression.MoveNext();
                    break;
                }
                case LevelStatus.finished:
                {
                    on_game_complete();
                    break;
                }
                default:
                {
                    break;
                }

            }

        }

        private void on_game_complete()
        {
            StartCoroutine(celebrate_then_go_to_game_complete_screen());
        }

        private IEnumerator celebrate_then_go_to_game_complete_screen()
        {
            clear_wave();
            Bullet.clear_bullets_from_game();
            timeout.stop();
            launch_background(default_background_prefab);

            const int random_splosions_batch_count = 42;
            const int random_splosions_per_batch = 3;

            for (int batch_idx = 0; batch_idx < random_splosions_batch_count; ++batch_idx)
            {
                for (int splosion_idx = 0; splosion_idx < random_splosions_per_batch; ++splosion_idx)
                {
                    var random_pos = Wave_Spawning.random_position_in_screen();
                    var random_splosion_prefab = game_complete_splosions_prefabs[Random.Range(0, game_complete_splosions_prefabs.Count)];
                    Instantiate(random_splosion_prefab, random_pos, Quaternion.identity);
                }

                yield return new WaitForSeconds(1.0f / 8.0f);
            }

            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("GameCompleteScreen", LoadSceneMode.Single);
        }

        private WaveInfo pick_a_wave_in(IList<Wave> wave_bag, WaveCategory wave_category = WaveCategory.Wave)
        {
            if (wave_bag == null || wave_bag.Count == 0)
            {
                Debug.LogWarningFormat("No ennemies in enemy wave bag: {0}", wave_bag);
                return null;
            }
            var random_idx = Random.Range(0, wave_bag.Count);
            var picked_wave = wave_bag[random_idx];
            WaveInfo result = new WaveInfo();
            result.wave = picked_wave;
            result.category = wave_category;
            return result;
        }

        private void clear_wave()
        {
            if (current_wave != null)
            {
                current_wave.finish();
                Destroy(current_wave.gameObject);
                current_wave = null;
            }
        }

        private IEnumerator start_wave(WaveInfo wave_info)
        {
            clear_wave();

            state = State.playing_wave;

            set_theme_color(wave_info.wave.background_color);
            launch_background(wave_info.wave.background);
            string progress_title = string.Format("Level {0} {2}- Wave {1}", current_level_number, current_wave_number, wave_info.category == WaveCategory.Boss ? "- Boss " : "");

            int wave_start_idx = ++wave_start_count; // Keep track of which wave we were starting.

            timeout.stop();
            if (wave_info.wave.timeout_secs > 0)
            {
                timeout.show_timeout(wave_info.wave.timeout_secs);
            }

            if (wave_info.wave.music_tracks)
            {
                MusicEventManager.Instance.Transition(wave_info.wave.music_tracks);
            }

            MusicEventManager.Instance.play_new_wave_sound();

            yield return display_title(progress_title, wave_info.wave.title, title_display_duration_secs);

            if (wave_start_idx != wave_start_count) // If another wave was started in-betwen, do nothing.
                yield break;

            current_wave = Instantiate(wave_info.wave);

            current_wave.on_finished += wave => next_wave();

            if (current_wave.timeout_secs > 0)
            {
                timeout.start(current_wave.timeout_secs);
            }
        }

        private void set_theme_color(Color color)
        {
            if (theme_color_progression != null)
            {
                StopCoroutine(theme_color_progression);
                theme_color_progression = null;
            }

            theme_color_progression = update_theme_color(color);
            StartCoroutine(theme_color_progression);
        }

        private IEnumerator update_theme_color(Color color)
        {
            var initial_color = Camera.main.backgroundColor;
            float progression = 0.0f;
            do
            {
                progression += color_change_speed * Time.deltaTime;
                var new_color = Color.Lerp(initial_color, color, progression);

                Camera.main.backgroundColor = new_color;
                RenderSettings.skybox.color = new_color;
                RenderSettings.skybox.SetColor("_Color", new_color);
                if (RenderSettings.skybox.HasProperty("_Tint"))
                    RenderSettings.skybox.SetColor("_Tint", new_color);
                else if (RenderSettings.skybox.HasProperty("_SkyTint"))
                    RenderSettings.skybox.SetColor("_SkyTint", new_color);

                RenderSettings.fogColor = new_color;

                yield return new WaitForEndOfFrame();
            }
            while (progression < 1);
        }

        private void launch_background(Background background_prefab)
        {
            if (current_background)
            {
                current_background.on_wave_end();
                current_background = null;
            }

            if (background_prefab == null)
                return;

            var background = GameObject.Instantiate<Background>(background_prefab, Vector3.zero, Quaternion.identity);
            background.on_wave_begin();

            current_background = background;
        }

        private void display_game_title()
        {
            StartCoroutine(display_title("", DEFAULT_TITLE, default_title_display_duration_secs));

            MusicEventManager.Instance.play_title_sound();
        }

        private IEnumerator display_title(string progress_text, string title_text, float duration_secs)
        {
            if (title_display == null || progress_display == null)
            {
                Debug.LogError("Incomplete text set to display the title!");
                yield break;
            }

            int title_display_idx = ++title_display_count; // Keep track of which title display request we correspond to.

            Debug.LogFormat("Progress: {0}", progress_text);
            Debug.LogFormat("Title: {0}", title_text);
            title_display.text = title_text;
            title_display.enabled = true;
            progress_display.text = progress_text;
            progress_display.enabled = true;

            yield return new WaitForSeconds(duration_secs);
            if (title_display_idx != title_display_count) // No other title display was launched in between, otherwise we do nothing.
                yield break;

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
                if (current_level_waves_selection == null)
                    break;

                yield return LevelStatus.next_level;

                current_wave_number = 0;
                foreach (WaveInfo wave_info in current_level_waves_selection)
                {
                    ++current_wave_number;
                    start_wave_coroutine = start_wave(wave_info);
                    StartCoroutine(start_wave_coroutine);
                    yield return LevelStatus.next_wave;
                }
            }

            yield return LevelStatus.finished;
        }


        private static void store_if_any<T>(IList<T> result_list, T x)
        {
            if (x != null)
            {
                result_list.Add(x);
            }
        }

        private List<WaveInfo> build_level(int level_number)
        {
            if (level_number < 1)
            {
                Debug.LogErrorFormat("Wrong level number: {0}", level_number);
                return null;
            }

            List<WaveInfo> selected_waves = new List<WaveInfo>();

            switch (level_number)
            {
                case 1:
                    {
                        store_if_any(selected_waves, pick_a_wave_in(level_1_waves_1));
                        store_if_any(selected_waves, pick_a_wave_in(level_1_waves_2));
                        store_if_any(selected_waves, pick_a_wave_in(level_1_waves_3));
                        store_if_any(selected_waves, pick_a_wave_in(level_1_waves_4));
                        store_if_any(selected_waves, pick_a_wave_in(level_1_waves_5));
                        store_if_any(selected_waves, pick_a_wave_in(level_1_waves_6));
                        store_if_any(selected_waves, pick_a_wave_in(level_1_boss, WaveCategory.Boss));
                        break;
                    }
                //case 2:
                //    {
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_1_easy));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_1_easy));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                //        selected_waves.Add(pick_a_wave_in(boss_lvl_2_hard, WaveCategory.Boss));
                //        break;
                //    }
                //case 3:
                //    {
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                //        selected_waves.Add(pick_a_wave_in(boss_lvl_2_hard, WaveCategory.Boss));
                //        break;
                //    }
                //case 4:
                //    {
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_2_challenging));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_3_hard));
                //        selected_waves.Add(pick_a_wave_in(waves_lvl_1_easy));
                //        selected_waves.Add(pick_a_wave_in(boss_lvl_1_challenging, WaveCategory.Boss));
                //        selected_waves.Add(pick_a_wave_in(boss_lvl_2_hard, WaveCategory.Boss));
                //        selected_waves.Add(pick_a_wave_in(boss_lvl_2_hard, WaveCategory.Boss));
                //        selected_waves.Add(pick_a_wave_in(boss_lvl_3_hardcore, WaveCategory.Boss));
                //        break;
                //    }
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
