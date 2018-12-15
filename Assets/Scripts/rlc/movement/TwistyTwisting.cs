using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistyTwisting : MonoBehaviour {

    public float time_between_rotation_changes = 0f;
    public bool randomize_time_between_rotations = false;
    public float min_time_between_rotation_changes = 0f;
    public float max_time_between_rotation_changes = 0f;

    public Vector3 min_rotation = new Vector3(1f, 1f, 1f);
    public Vector3 max_rotation = new Vector3(1000f, 1000f, 1000f);


    private Vector3 current_rotation;
    private float next_rotation_change_time = 0f;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

        var now = Time.time;
        if (now >= next_rotation_change_time)
        {
            current_rotation = random_rotation();

            if (randomize_time_between_rotations)
                time_between_rotation_changes = Random.Range(min_time_between_rotation_changes, max_time_between_rotation_changes);

            next_rotation_change_time = now + time_between_rotation_changes;
        }

        transform.rotation = transform.rotation * Quaternion.Euler(current_rotation * Time.deltaTime);
    }

    private Vector3 random_rotation()
    {
        var rotation_x = Random.Range(min_rotation.x, max_rotation.x);
        var rotation_y = Random.Range(min_rotation.y, max_rotation.y);
        var rotation_z = Random.Range(min_rotation.z, max_rotation.z);
        return new Vector3(rotation_x, rotation_y, rotation_z);
    }
}
