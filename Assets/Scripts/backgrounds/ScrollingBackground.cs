﻿using UnityEngine;
using System.Collections;

namespace rlc
{

    public class ScrollingBackground : Background
    {
        public GameObject tile_prefab;

        public Vector2 overlap;
        public Vector3 offset;
        public Vector3 velocity;
        public Vector3 acceleration;
        public float max_speed = 100.0f;

        private GameObject[] tiles;
        private float tile_size;
        private float half_tile_size;
        private int tiles_per_side = 0;
        private float side_size = 0;
        private float half_side_size = 0;
        private float double_side_size = 0;
        private float tile_adjust_offset = 0;
        private float tile_limit = 0;
        private float offset_limit = 0;


        void Start()
        {
            if (tile_prefab == null)
                Debug.LogError("Missing tile prefab!");


            create_tile_grid();
        }

        void Update()
        {
            move();
            update_tiles_positions();
        }

        private void move()
        {
            velocity += acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, max_speed);

            offset += velocity;
            offset = warp_around_limit(offset, double_side_size, double_side_size);
        }


        private void create_tile_grid()
        {
            // Here we assume that the camera will always be a square.
            tile_size = calculate_size(tile_prefab);
            half_tile_size = tile_size / 2.0f;
            tiles_per_side = Mathf.CeilToInt((GameCamera.SIZE_PER_SIDE) / tile_size) + 2;
            side_size = tiles_per_side * tile_size;
            half_side_size = side_size / 2.0f;
            double_side_size = side_size * 2.0f;
            tile_limit = half_side_size + half_tile_size;
            tile_adjust_offset = half_side_size - half_tile_size;

            int required_tiles_count = tiles_per_side * tiles_per_side;

            tiles = new GameObject[required_tiles_count];

            for (int tile_idx = 0; tile_idx < tiles.Length; ++tile_idx)
            {
                tiles[tile_idx] = create_tile();

                const string name_format = "tile_{0}";
                tiles[tile_idx].name = string.Format(name_format, tile_idx);
            }

            update_tiles_positions();
        }

        private GameObject create_tile()
        {
            return GameObject.Instantiate(tile_prefab, this.transform);
        }

        private float warp_around_limit(float pos, float limit, float adjustment)
        {
            while (Mathf.Abs(pos) > limit)
            {
                float sign = Mathf.Sign(pos);
                float reversed_sign = -sign;
                float correction = reversed_sign * adjustment;
                pos += correction;
            }
            return pos;
        }

        private Vector3 warp_around_limit(Vector3 pos, float limit, float adjustment)
        {
            pos.x = warp_around_limit(pos.x, limit, adjustment);
            pos.y = warp_around_limit(pos.y, limit, adjustment);
            return pos;
        }

        private Vector3 tile_position(int tile_idx)
        {
            int grid_x = tile_idx % tiles_per_side;
            int grid_y = tile_idx / tiles_per_side;

            float x = grid_x * (tile_size + overlap.x);
            float y = -grid_y * (tile_size + overlap.y);

            x -= tile_adjust_offset;
            y += tile_adjust_offset;

            var pos = new Vector3(x, y, BACKGROUND_Z);
            pos += offset;

            pos = warp_around_limit(pos, tile_limit, side_size);

            return pos;
        }

        private void update_tiles_positions()
        {
            for (int tile_idx = 0; tile_idx < tiles.Length; ++tile_idx)
            {
                var new_pos = tile_position(tile_idx);
                tiles[tile_idx].transform.position = new_pos;
            }
        }

        private static float calculate_size(GameObject obj)
        {
            float size = 0.0f;
            var renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                size = Mathf.Max(renderer.bounds.size.x, renderer.bounds.size.y, size);
            }

            return size;
        }
    }

}