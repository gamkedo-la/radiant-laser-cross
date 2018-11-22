using rlc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {
    private Vector3 velocity;
    private Vector3 lastMove = Vector3.zero;

    public void MoveTowards(Vector2 direction2d, float speed)
    {
        var direction3d = new Vector3(direction2d.x, direction2d.y, 0f);
        MoveTowards(direction3d, speed);
    }

    public void MoveTowards(Vector3 direction, float speed)
    {
        if (direction == Vector3.zero)
        {
            this.velocity = Vector3.zero;
            return;
        }
        transform.forward = direction.normalized;
        MoveForward(speed);
    }

    public void MoveForward(float speed)
    {
        this.velocity = Forward * speed;
        lastMove = this.velocity * Time.fixedDeltaTime;
        transform.Translate(lastMove, Space.World);
    }

    public Transform Transform
    {
        get { return transform;  }
    }

    public Vector3 Forward
    {
        get { return transform.forward; }
    }

    public Vector3 Velocity
    {
        get { return velocity; }
    }
    
    public Vector3 LastMove
    {
        get { return lastMove; }
    }
}
