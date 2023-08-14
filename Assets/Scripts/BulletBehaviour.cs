using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Environment") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            //Debug.Log("Bullet Hit Environment");
        }
    }
}
