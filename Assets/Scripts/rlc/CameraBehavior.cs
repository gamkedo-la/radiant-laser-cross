 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{
    public class CameraBehavior : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            // force_square_game_view();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void force_square_game_view()
        {
            var camera = GetComponent<Camera>();
            camera.ResetAspect();
            camera.aspect = 1.0f;
            var sqare_view_rect = new Rect(0, 0, Screen.width, Screen.width);
            camera.pixelRect = sqare_view_rect;


        }
    }
}