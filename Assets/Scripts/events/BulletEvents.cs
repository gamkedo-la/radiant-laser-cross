using rlc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletEvents {
    // Bullet hit and colors match
    public delegate void HitAction(Bullet bullet, ColoredBody body_hit);
    public static event HitAction OnHit;

    // Bullet hit and colors don't match
    public delegate void AbsorvedAction(Bullet bullet, ColoredBody body_hit);
    public static event AbsorvedAction OnAbsorved;

    // Bullet was destroyed before hitting something
    public delegate void MissAction(Bullet bullet);
    public static event MissAction OnMiss;

    public static void InvokeOnHit(Bullet bullet, ColoredBody body_hit)
    {
        if(OnHit != null) OnHit(bullet, body_hit);
    }

    public static void InvokeOnAbsorved(Bullet bullet, ColoredBody body_hit)
    {
        if (OnAbsorved != null) OnAbsorved(bullet, body_hit);
    }

    public static void InvokeOnMiss(Bullet bullet)
    {
        if (OnMiss != null) OnMiss(bullet);
    }
}
