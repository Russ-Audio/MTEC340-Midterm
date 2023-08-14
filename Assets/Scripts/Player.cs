using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int _life = 3;
    public int Life
    {
        get
        {
            return _life;
        }

        set
        {
            _life = value;
        }
    }

    private int _point = 2;
    public int Point
    {
        get
        {
            return _point;
        }

        set
        {
            _point = value;
        }
    }
}
