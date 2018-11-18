using UnityEngine;
using System.Collections;

namespace rlc
{

    public class ScrollingBackground : Background
    {
        public GameObject tile_prefab;
        public float tile_size = GameCamera.SIZE_PER_HALF_SIDE;

        public float scrolling_angle_from_up_degs = 180.0f;
        public float current_offset = 0.0f;
        public float speed = 10.0f;
        public float max_speed = 100.0f;
        public float acceleration = 0.1f;

        private GameObject[] tiles;

        void Start()
        {
            if (tile_prefab == null)
                Debug.LogError("Missing tile prefab!");



        }

        void Update()
        {


            update_tiles_positions();
        }


        private void create_tile_grid()
        {
            // Here we assume that the camera will always be a square.
            int tiles_per_side = Mathf.CeilToInt(GameCamera.SIZE_PER_SIDE / tile_size);
            int required_tiles_count = tiles_per_side * tiles_per_side;

            tiles = new GameObject[required_tiles_count];

            for (int tile_idx = 0; tile_idx < tiles.Length; ++tile_idx)
            {
                tiles[tile_idx] = create_tile();
            }

            update_tiles_positions();
        }

            private GameObject create_tile()
        {
            return GameObject.Instantiate(tile_prefab, this.transform);
        }

        private void update_tiles_positions()
        {

        }

    }

}