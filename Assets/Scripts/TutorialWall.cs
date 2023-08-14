using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWall : MonoBehaviour
{
    public TutorialManager tutorial;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet") && tutorial.GetReady == true)
        {
            //play breaking audio sound
            tutorial.WallHit();
        }
    }
}
