﻿using rlc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour {
    private const int MAX_SCORE_MULTIPLIER = 5;
    private const int AMOUNT_TO_INCREASE_MULTIPLIER = 5;
    private int shot_sequence = 0;

    void Start() {
        BulletEvents.OnHit += OnBulletHit;
        BulletEvents.OnAbsorved += OnBulletAbsorved;
        BulletEvents.OnMiss += OnBulletMiss;
    }

    public int ScoreMultiplier {
        get
        {
            if (shot_sequence == 0) return 1; // prevent from zero division
            else return Mathf.Min(MAX_SCORE_MULTIPLIER, shot_sequence / AMOUNT_TO_INCREASE_MULTIPLIER + 1); // Each five shots it gets +1 on multiplier
        }
    }

    // When this count achieve AMOUNT_TO_INCREASE_MULTIPLIER the ScoreMultiplier is increased
    public int ScoreMultiplierCount
    {
        get
        {
            if (ScoreMultiplier == MAX_SCORE_MULTIPLIER)
            {
                // if player are at the max multiplier the count will be always at maximum
                return AMOUNT_TO_INCREASE_MULTIPLIER;
            }
            else
            {
                return shot_sequence % AMOUNT_TO_INCREASE_MULTIPLIER;
            }
        }
    }

    private void UpdateDisplay()
    {
        Debug.LogFormat("Sequence: {0}; Multiplier: {1}; Count: {2}", shot_sequence, ScoreMultiplier, ScoreMultiplierCount);
    }

    private void OnBulletHit(Bullet bullet, ColoredBody body_hit)
    {
        if(bullet.is_player_responsability())
        {
            shot_sequence += 1;
            UpdateDisplay();
        }
    }

    private void OnBulletAbsorved(Bullet bullet, ColoredBody body_hit)
    {
        if (bullet.is_player_responsability())
        {
            shot_sequence = 0;
            UpdateDisplay();
        }
    }

    private void OnBulletMiss(Bullet bullet)
    {
        if (bullet.is_player_responsability())
        {
            shot_sequence = 0;
            UpdateDisplay();
        }
    }
}
