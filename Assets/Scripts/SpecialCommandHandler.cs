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
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyUp(KeyCode.Space))
            level_builder.new_game();

        if (Input.GetKeyUp(KeyCode.R))
            level_builder.game_over(rlc.GameOverReason.hard_reset);

        if (Input.GetKeyUp(KeyCode.Tab))
            level_builder.next_wave();

    }
}
