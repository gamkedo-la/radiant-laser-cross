using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAssets : MonoBehaviour {

    public GameObject spawning_box;

    public GameObject bullet;

    public static GeneralAssets instance;

    private GameObject[] bullet_pool;
    private int next_bullet_idx = 0;

    void Start()
    {
        instance = this;

        //const int bullet_count = 10000;
        //bullet_pool = new GameObject[bullet_count];
        //for (int idx = 0; idx < bullet_count; ++idx)
        //{
        //    var new_bullet = Instantiate(bullet);
        //    new_bullet.SetActive(false);
        //    bullet_pool[idx] = new_bullet;
        //}
    }


    public GameObject make_bullet(Vector3 position, Quaternion rotation)
    {
        //var new_bullet = bullet_pool[next_bullet_idx];
        //++next_bullet_idx;
        //new_bullet.transform.position = position;
        //new_bullet.transform.rotation = rotation;
        //new_bullet.SetActive(true);
        //return new_bullet;
        return Instantiate(bullet, position, rotation);
    }

}
