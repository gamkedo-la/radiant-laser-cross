using rlc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour {
    
	void Start () {
        BulletEvents.OnHit += OnBulletHit;
        BulletEvents.OnAbsorved += OnBulletAbsorved;
        BulletEvents.OnMiss += OnBulletMiss;
    }

    private void OnBulletHit(Bullet bullet, ColoredBody body_hit)
    {
        Debug.Log("detected event BulletHit!");
    }

    private void OnBulletAbsorved(Bullet bullet, ColoredBody body_hit)
    {
        Debug.Log("detected event BulletAbsorved!");
    }

    private void OnBulletMiss(Bullet bullet)
    {
        Debug.Log("detected event BulletMiss!");
    }
}
