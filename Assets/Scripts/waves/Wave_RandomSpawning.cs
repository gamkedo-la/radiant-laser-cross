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
        public int count_left_to_spawn = 10;
        public int instances_per_spawn = 2;
        public float spawn_interval_secs = 1.0f;

        private float last_spawn_time;

        // Use this for initialization
        void Start()
        {
            last_spawn_time = Time.time;
            setting_up();

            StartCoroutine(update_spawn());
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
                var random_wall = (AxesDirections)(Random.Range(0, System.Enum.GetValues(typeof(AxesDirections)).Length));
                var orientation = -border_from_screen_center(random_wall);
                spawn_enemy(random_prefab(), random_position(random_wall), Quaternion.LookRotation(orientation, Vector3.back));
                --instance_count_to_spawn;
            }

            if (count_left_to_spawn == 0)
            {
                ready();
            }
        }

        private const float border_distance_from_center = 35.0f; // TODO: replace by something deduced from actual data, not a guess\
        private const float half_border_distance_from_center = border_distance_from_center; // TODO: replace by something deduced from actual data, not a guess

        private Vector3 random_position(AxesDirections wall)
        {
            var wall_position = border_from_screen_center(wall);
            var random_side_step = Random.Range(-half_border_distance_from_center, half_border_distance_from_center);
            switch (wall)
            {
                case AxesDirections.north:  return wall_position + new Vector3(random_side_step, 0, 0);
                case AxesDirections.east:   return wall_position + new Vector3(0, random_side_step, 0);
                case AxesDirections.south:  return wall_position + new Vector3(random_side_step, 0, 0);
                case AxesDirections.west:   return wall_position + new Vector3(0, random_side_step, 0);
                default:
                    return new Vector3(0, 0, 0); // Unmanaged case: make it fail (TODO)
            }
        }

        private Vector3 border_from_screen_center(AxesDirections border_direction)
        {
            switch (border_direction)
            {
                case AxesDirections.north:  return new Vector3(0, border_distance_from_center, 0);
                case AxesDirections.east:   return new Vector3(border_distance_from_center, 0, 0);
                case AxesDirections.south:  return new Vector3(0, -border_distance_from_center, 0);
                case AxesDirections.west:   return new Vector3(-border_distance_from_center, 0, 0);
                default:
                    return new Vector3(0, 0, 0); // Unmanaged case: make it fail (TODO)
            }
        }

        private GameObject random_prefab()
        {
            var random_idx = Random.Range(0, prefabs_to_spawn.Count);
            return prefabs_to_spawn[random_idx];
        }

    }
}