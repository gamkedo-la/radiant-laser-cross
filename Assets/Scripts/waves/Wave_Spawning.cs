﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    // Spawns enemies at random screen border positions
    public class Wave_Spawning : Wave
    {
        public enum Mode
        {
            random, sequential
        }

        public Mode spawn_mode = Mode.random;
        public Mode selection_mode = Mode.random;

        public List<GameObject> prefabs_to_spawn;
        public List<Transform> spawn_points;

        public bool target_player = false;
        public bool reuse_spawn_points = false;
        public int count_left_to_spawn = 10;
        public int instances_per_prefab = 1;
        public int instances_per_spawn = 2;
        public float spawn_interval_secs = 1.0f;
        public const float margin_from_max = 10.0f;
        public const float max_distance_from_center = GameCamera.SIZE_PER_HALF_SIDE - margin_from_max;

        private List<Transform> used_spawn_points = new List<Transform>();
        private bool started = false;

        private struct SpawnState
        {
            public Vector3 position;
            public Quaternion rotation;
        };

        // Use this for initialization
        void Start()
        {
            setting_up();

            // Make sure the spawn points are not visible.
            foreach (var spawn_point in spawn_points)
            {
                spawn_point.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!started)
            {
                started = true;
                StartCoroutine(update_spawn());
            }
        }

        private IEnumerator update_spawn()
        {
            do
            {
                spawn();
                yield return new WaitForSeconds(spawn_interval_secs);
            }
            while (count_left_to_spawn > 0);
        }

        private void spawn()
        {
            var instance_count_to_spawn = instances_per_spawn;
            if (instance_count_to_spawn > count_left_to_spawn)
            {
                instance_count_to_spawn = count_left_to_spawn;
            }

            count_left_to_spawn -= instance_count_to_spawn;

            int count_instances_with_selected_prefab = 0;
            GameObject selected_prefab = select_prefab();

            while (instance_count_to_spawn > 0)
            {
                if (count_instances_with_selected_prefab >= instances_per_prefab)
                {
                    // Time to change prefab.
                    selected_prefab = select_prefab();
                    count_instances_with_selected_prefab = 0;
                } // Otherwise, keep the same prefab than the previous one.

                var spawn_transform = next_spawn_state();

                spawn_enemy(selected_prefab, spawn_transform.position, spawn_transform.rotation);
                --instance_count_to_spawn;
                ++count_instances_with_selected_prefab;
            }

            if (count_left_to_spawn == 0)
            {
                ready();
            }
        }

        public static Vector3 random_position_in_screen()
        {
            return new Vector3(Random.Range(-max_distance_from_center, max_distance_from_center), Random.Range(-max_distance_from_center, max_distance_from_center), 0);
        }

        private SpawnState next_spawn_state()
        {
            switch (spawn_mode)
            {
                case Mode.random: return random_transform();
                case Mode.sequential: return sequential_transform();
                default:
                    Debug.LogError("Invalid Spawn Mode in Randomly Spawning Wave!");
                    return random_transform();
            }

        }

        private SpawnState sequential_transform()
        {
            if (spawn_points == null || spawn_points.Count == 0)
            {
                Debug.LogError("No spawn points set in Sequential mode!");
                return random_transform();
            }

            var next_spawn_point = spawn_points[0];
            Utility.RotateForward(spawn_points, 1); // Make sure the next spawn point we pick will be the following one.

            var spawn_state = new SpawnState();
            spawn_state.position = next_spawn_point.position;

            if (target_player)
            {
                spawn_state.rotation = rotation_look_at_player(spawn_state.position);
            }
            else
            {
                spawn_state.rotation = next_spawn_point.rotation;
            }

            return spawn_state;
        }

        private SpawnState random_transform()
        {
            var spawn_state = new SpawnState();

            // If we have spawn points, use them. Otherwise, just use a direction opposite from the center so that we go through the screen..
            if (spawn_points != null
            && (spawn_points.Count > 0 || used_spawn_points.Count > 0))
            {
                if (spawn_points.Count == 0)
                {
                    spawn_points.AddRange(used_spawn_points);
                    used_spawn_points.Clear();
                }


                var random_idx = Random.Range(0, spawn_points.Count);
                var spawn_point = spawn_points[random_idx];
                if (!reuse_spawn_points)
                {
                    used_spawn_points.Add(spawn_point);
                    spawn_points.RemoveAt(random_idx);
                }

                spawn_state.position = spawn_point.position;

                if (target_player)
                {
                    spawn_state.rotation = rotation_look_at_player(spawn_state.position);
                }
                else
                {
                    spawn_state.rotation = spawn_point.rotation;
                }
            }
            else
            {
                spawn_state.position = new Vector3(Random.Range(-max_distance_from_center, max_distance_from_center), Random.Range(-max_distance_from_center, max_distance_from_center), 0);

                if (target_player)
                {
                    spawn_state.rotation = rotation_look_at_player(spawn_state.position);
                }
                else
                {
                    float symetry_x = Random.Range(0.0f, 1.0f);
                    float symetry_y = Random.Range(0.0f, 1.0f);
                    var destination = new Vector3(spawn_state.position.x * symetry_x, spawn_state.position.y * symetry_y, 0);
                    var opposite_direction = destination - spawn_state.position;
                    spawn_state.rotation = Quaternion.LookRotation(opposite_direction, Vector3.back);
                }
            }

            return spawn_state;
        }

        private Quaternion rotation_look_at_player(Vector3 from_pos)
        {
            var player_direction = LaserCross.current.transform.position - from_pos;
            return Quaternion.LookRotation(player_direction, Vector3.back);
        }

        private GameObject random_prefab()
        {
            var random_idx = Random.Range(0, prefabs_to_spawn.Count);
            return prefabs_to_spawn[random_idx];
        }

        private GameObject next_prefab()
        {
            var selected_prefab = prefabs_to_spawn[0];
            Utility.RotateForward(prefabs_to_spawn, 1);
            return selected_prefab;
        }

        private GameObject select_prefab()
        {
            switch (selection_mode)
            {
                case Mode.random: return random_prefab();
                case Mode.sequential: return next_prefab();
                default:
                    Debug.LogError("Invalid Selection Mode in Randomly Spawning Wave!");
                    return random_prefab();
            }
        }

    }
}