using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTutorial : MonoBehaviour
{
    public TutorialManager tutorial;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (transform.position.x <= 0)
            {
                tutorial.NextCheckPointPlayer1();
            }
            else
            {
                tutorial.NextCheckPointPlayer2();
            }
        }
    }
}
