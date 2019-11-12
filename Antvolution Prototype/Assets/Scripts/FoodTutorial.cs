﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodTutorial : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float fadeInSpeed = 0.01f;
    public float minimumTime = 5.0f;
    public Image image;
    public GameObject next;
    private Color c;
    private float startTime;
    private bool leftClick;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        leftClick = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            leftClick = true;
        }

        if (Time.time - startTime >= minimumTime && leftClick)
        {
            c = text.color;
            c.a -= fadeInSpeed;
            text.color = c;
            image.color = c;
            if (c.a <= 0)
            {
                if (next)
                {
                    next.SetActive(true);
                }
                Destroy(this.gameObject);
            }
        }
        else if (c.a < 1)
        {
            c = text.color;
            c.a += fadeInSpeed;
            text.color = c;
            image.color = c;
        }
    }
}
