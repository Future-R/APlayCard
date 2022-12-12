using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text timeText;
    private float timer;

    void Start()
    {
        timer = 0f;
        timeText = GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        timeText.text = timer.ToString("f2");
    }
}
