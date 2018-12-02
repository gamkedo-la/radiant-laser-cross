using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCommandHandler : MonoBehaviour {

    private rlc.ProceduralLevelBuilder level_builder;
    private float last_wave_change_time;
    private const float time_between_wave_change = 1.0f;
    private bool deactivate_input = false;

    // Use this for initialization
    void Start () {
        level_builder = GetComponentInChildren<rlc.ProceduralLevelBuilder>();
        last_wave_change_time = Time.time;
    }

    // Update is called once per frame
    void Update () {

        if (deactivate_input)
            return;

        var now = Time.time;
        var time_since_last_wave_change = now - last_wave_change_time;

        if (Input.GetKeyUp(KeyCode.Escape)
        ||  Input.GetAxisRaw("DPAD_HORIZ") == -1) // DPAD left
        {
            deactivate_input = true;
            level_builder.exit();
        }

        if (Input.GetKeyUp(KeyCode.Space)
        ||  Input.GetKeyDown("joystick 1 button 7")) // gamepad start button
        {
            level_builder.new_game();
        }

        if (Input.GetKeyUp(KeyCode.R) ||
            Input.GetKeyDown("joystick 1 button 6")) // gamepad back button
        {
            if (time_since_last_wave_change >= time_between_wave_change)
            {
                last_wave_change_time = Time.time;
                level_builder.game_over(rlc.GameOverReason.hard_reset);
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab)
        ||  Input.GetAxis("DPAD_HORIZ") == 1) // DPAD right
        {

            if (time_since_last_wave_change >= time_between_wave_change)
            {
                last_wave_change_time = Time.time;
                level_builder.next_wave();
                rlc.Bullet.clear_bullets_from_game();
            }
        }

        // Reactivate this for debugging
        // Gamepad Debug [to figure out which button # is which]
        //for (int i = 0; i < 20; i++)
        //{
        //    if (Input.GetKeyDown("joystick 1 button " + i))
        //    {
        //        print("joystick 1 button " + i + " pressed");
        //    }
        //}

    }
}
