using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShutOff : MonoBehaviour
{
    public BoxCollider2D laser_1;
    public BoxCollider2D laser_2;

    private void Start()
    {
        laser_1.enabled = true;
        laser_2.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.gameObject.CompareTag("Player"))
            {
                laser_1.enabled = false;
                laser_2.enabled = laser_1.enabled;
            }
    }
}
