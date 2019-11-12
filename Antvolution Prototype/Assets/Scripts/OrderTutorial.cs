using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderTutorial : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public float fadeInSpeed = 0.01f;
    public float minimumTime = 3.0f;
    public Image image;
    public Image image2;
    public Image image3;
    public Image image4;
    public Image image5;
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
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            leftClick = true;
        }


        //TO DO
        if (Time.time - startTime >= minimumTime && leftClick)
        {
            c = text.color;
            c.a -= fadeInSpeed;
            text.color = c;
            image.color = c;
            if (c.a <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
