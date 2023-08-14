using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Flashing : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tm;
    [SerializeField] float flashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _tm = GetComponent<TextMeshProUGUI>();
        _tm.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float sineFlashTime = Mathf.Sin(Time.time * flashSpeed) * 0.5f;
        if(Mathf.CeilToInt(sineFlashTime) == 1)
        {
            _tm.enabled = true;
        }
        else
        {
            _tm.enabled = false;
        }
    }
}
