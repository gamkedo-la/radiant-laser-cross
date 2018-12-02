using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCommandHandler : MonoBehaviour {

    rlc.ProceduralLevelBuilder level_builder;

    // Use this for initialization
    void Start () {
        level_builder = GetComponentInChildren<rlc.ProceduralLevelBuilder>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Escape) ||
            //Input.GetKeyDown("joystick 1 button 4")) // gamepad LB
            (Input.GetAxis("DPAD_HORIZ") < -0.5)) // DPAD left
        {
            level_builder.exit();
        }

        if (Input.GetKeyUp(KeyCode.Space) ||
            Input.GetKeyDown("joystick 1 button 7")) // gamepad start button
        {
            level_builder.new_game();
        }

        if (Input.GetKeyUp(KeyCode.R) ||
            //Input.GetKeyDown("joystick 1 button 4")) // gamepad LB
            Input.GetKeyDown("joystick 1 button 6")) // gamepad back button
        {
            level_builder.game_over(rlc.GameOverReason.hard_reset);
        }

        if (Input.GetKeyUp(KeyCode.Tab) ||
            //Input.GetKeyDown("joystick 1 button 5")) // gamepad RB
            (Input.GetAxis("DPAD_HORIZ") > 0.5)) // DPAD right
        {
            level_builder.next_wave();
            rlc.Bullet.clear_bullets_from_game();
        }

         
        // Gamepad Debug [to figure out which button # is which]
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick 1 button " + i))
            {
                print("joystick 1 button " + i + " pressed");
            }
        }

    }
}
