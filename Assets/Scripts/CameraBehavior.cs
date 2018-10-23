using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Vector2Int screen_resolution;
    public int min_square_side_size = 480;
    public int min_side_padding = 100;

    // Use this for initialization
    void Start()
    {
        force_square_game_view();
    }

    // Update is called once per frame
    void Update()
    {
        if (screen_resolution.x != Screen.currentResolution.width
        || screen_resolution.y != Screen.currentResolution.height)
        {
            force_square_game_view();
        }
    }

    private void force_square_game_view()
    {
        screen_resolution = new Vector2Int(Screen.width, Screen.height);

        int square_side = Math.Max(min_square_side_size, Screen.height);
        int potential_padding = (Screen.width - square_side) / 2;
        int side_padding = Mathf.Max(min_side_padding, potential_padding);

        var half_square_side = square_side / 2;
        var pos_x = (Screen.width / 2) - half_square_side;
        var pos_y = (Screen.height / 2) - half_square_side;
        var sqare_view_rect = new Rect(pos_x, pos_y, square_side, square_side);

        var camera = GetComponent<Camera>();
        camera.pixelRect = sqare_view_rect;
    }

}
