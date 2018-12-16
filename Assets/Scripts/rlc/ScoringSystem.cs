using rlc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    public class ScoringSystem : MonoBehaviour
    {
        private const int MAX_SCORE_MULTIPLIER = 5;
        private const int AMOUNT_TO_INCREASE_MULTIPLIER = 5;
        private int shot_sequence = 0;
        private int score = 0;

        private static class TimeBonus
        {
            public struct TimeBonusStruct
            {
                public readonly string name;
                public readonly float timelimit;
                public readonly int points_amount;

                public TimeBonusStruct(string name, float timelimit, int points_amount)
                {
                    this.name = name;
                    this.timelimit = timelimit;
                    this.points_amount = points_amount;
                }
            }

            public static TimeBonusStruct[] BONUSES = {
            new TimeBonusStruct(name: "Crazy Fast", timelimit: 5f, points_amount: 100),
            new TimeBonusStruct(name: "Very Fast", timelimit: 10f, points_amount: 50),
            new TimeBonusStruct(name: "Fast", timelimit: 15f, points_amount: 20),
        };

            public static TimeBonusStruct NOBONUS = new TimeBonusStruct(name: "", timelimit: 0f, points_amount: 0);

            public static TimeBonusStruct GetBonusForTime(float time)
            {
                foreach (var bonus in BONUSES)
                {
                    if (time < bonus.timelimit)
                        return bonus;
                }
                return NOBONUS;
            }
        }

        void Start()
        {
            BulletEvents.OnHit += OnBulletHit;
            BulletEvents.OnAbsorved += OnBulletAbsorved;
            BulletEvents.OnMiss += OnBulletMiss;
            EnemyEvents.OnKilled += OnEnemyKilled;
        }

        public void reset()
        {
            shot_sequence = 0;
            score = 0;
            UpdateDisplay();
        }

        public int ScoreMultiplier
        {
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
            UI_ScoringMultiplier.DisplayMultiplier(ScoreMultiplier);
            UI_ScoringMultiplier.DisplaySequenceCount(ScoreMultiplierCount);
            UI_Scoring.DisplaySequenceCount(score);
            // Debug.LogFormat("Sequence: {0}; Multiplier: {1}; Count: {2}", shot_sequence, ScoreMultiplier, ScoreMultiplierCount);
        }

        private void OnBulletHit(Bullet bullet, ColoredBody body_hit)
        {
            if (bullet.is_player_responsability())
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

        private void OnEnemyKilled(LifeControl life)
        {
            var time_bonus = TimeBonus.GetBonusForTime(life.ScreenTime);
            var enemy_score = life.score_points_on_destroyed <= 0 ? life.TotalHitPoints : life.score_points_on_destroyed;
            var kill_points = time_bonus.points_amount + enemy_score;
            var total_points = kill_points * ScoreMultiplier;
            var screenPoint = life.gameObject.transform.position;
            UI_Scoring.DisplayEnemyPoint(screenPoint, enemy_score, time_bonus.name);
            score += total_points;
            UpdateDisplay();
        }
    }
}