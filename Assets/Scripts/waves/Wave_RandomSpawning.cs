using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    // Spawns enemies at random screen border positions
    public class Wave_RandomSpawning : Wave
    {
        // TODO: this is just a quick demo, unfinished

        public List<GameObject> prefabs_to_spawn;
        public List<Transform> spawn_points;

        public bool target_player = false;
        public bool reuse_spawn_points = false;
        public int count_left_to_spawn = 10;
        public int instances_per_spawn = 2;
        public float spawn_interval_secs = 1.0f;
        public const float max_distance_from_center = 35.0f; // TODO: replace by something deduced from actual data, not a guess

        private List<Transform> used_spawn_points = new List<Transform>();

        private struct SpawnState
        {
            public Vector3 position;
            public Quaternion rotation;
        };

        // Use this for initialization
        void Start()
        {
            setting_up();
            StartCoroutine(update_spawn());

            // Make sure the spawn points are not visible.
            foreach (var spawn_point in spawn_points)
            {
                spawn_point.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
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

            while (instance_count_to_spawn > 0)
            {
                var spawn_transform = random_transform();
                spawn_enemy(random_prefab(), spawn_transform.position, spawn_transform.rotation);
                --instance_count_to_spawn;
            }

            if (count_left_to_spawn == 0)
            {
                ready();
            }
        }

        private SpawnState random_transform()
        {
            var spawn_state = new SpawnState();

            // If we have spawn points, use them. Otherwise, just use truly random positions in or on the border of the screen.
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
                    var random_direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized;
                    spawn_state.rotation = Quaternion.LookRotation(random_direction, Vector3.back);
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

    }
}