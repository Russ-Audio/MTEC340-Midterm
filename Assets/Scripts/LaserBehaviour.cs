using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    public float LaserSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0f, 0f, LaserSpeed);
    }
}
