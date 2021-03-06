﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace rlc
{
    /** For things that have life and can be destroyed/killed (can take hits etc.)
     * */
    public class LifeControl : MonoBehaviour
    {

        private enum LifeState
        {
            alive_newborn,      // Alive but not entered screen yet.
            alive_in_screen,    // Alive and in screen.
            dying               // Dying (doing it's dying animation).
        }
        [SerializeField]
        private LifeState life_state = LifeState.alive_newborn; // TODO: read-only public access

        public int score_points_on_destroyed = 100;
        public int hit_points = 1;
        private int total_hit_points;
        public List<ColoredBody> body_parts = new List<ColoredBody>();

        public delegate void LifeEvent(LifeControl life);
        public LifeEvent on_destroyed;


        public GameObject explosionPrefab;
        public GameObject hitPrefab;
        public float start_time;
        public float screen_enter_time = -100f;

        public float diying_duration = 0.5f;

        // Use this for initialization
        void Start()
        {
            total_hit_points = hit_points;
            if (body_parts.Count == 0)
            {
                Debug.LogErrorFormat("Destructible object {0} have no body parts set!", this.name);
            }

            foreach(var body in body_parts)
            {
                if (body.life_control != null)
                {
                    Debug.LogError("Body associated with more than one body part", this);
                }
                body.life_control = this;
            }

            start_time = Time.time;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool is_alive()
        {
            return life_state != LifeState.dying;
        }

        public bool is_alive_in_screen()
        {
            return life_state == LifeState.alive_in_screen;
        }

        public bool is_newborn()
        {
            return life_state == LifeState.alive_newborn;
        }

        public void on_hit(ColoredBody body_part_hit) // TODO: add the position of the hit point
        {
            //Debug.Log("on_hit!");

            if (!is_alive_in_screen())
                return;

            --hit_points;
            if (hit_points <= 0)
            {
                //Debug.Log("on_hit: die!");
                die();
            }
            else
            {
                HitFX(body_part_hit.transform);
            }
        }

        public void die() // Called only if the enemy is supposed to be destroyed by being killed, not when it's destroyed for other reasons.
        {
            life_state = LifeState.dying;
            Destroy(gameObject, diying_duration);
            launch_destruction_animation();

            if (this.gameObject.CompareTag(Wave.ENEMY_TAG))
                EnemyEvents.InvokeOnKilled(this);
        }

        public void on_entered_screen()
        {
            life_state = LifeState.alive_in_screen;
            if (screen_enter_time < 0f)
            {
                screen_enter_time = Time.time;
            }
        }

        public void launch_destruction_animation()
        {
            foreach (var body_part in GetComponentsInChildren<ColoredBody>())
            {
                ExplodeFX(body_part.transform);
            }
            ExplodeFX(this.transform);
        }

        public void ExplodeFX(Transform fx_transform)
        {
            launch_fx(explosionPrefab, fx_transform.position, fx_transform.rotation, 5);
        }

        public void HitFX(Transform fx_transform)
        {
            launch_fx(hitPrefab, fx_transform.position, fx_transform.rotation, 5);
        }

        private void launch_fx(GameObject prefab, Vector3 position, Quaternion rotation, float max_duration)
        {
            if (prefab)
            {
                GameObject fx = Instantiate(prefab, position, rotation);
                Destroy(fx, max_duration);

                // To make sure that we always see the fx over the related object, move the fx
                // in front of the camera.
                const float translation_size_to_the_camera = 1;
                fx.transform.Translate(Vector3.back * translation_size_to_the_camera, Space.World);
            }
        }

        private void OnDestroy()
        {
            notify_destroyed();
        }

        private void notify_destroyed()
        {
            if (on_destroyed != null)
                on_destroyed(this);
        }

        public float ScreenTime
        {
            get { return screen_enter_time < 0f ? 0f : Time.time - screen_enter_time; }
        }

        public int TotalHitPoints
        {
            get { return total_hit_points; }
        }
    }
}