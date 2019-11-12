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
    private Color p;
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
            p = this.GetComponent<Image>().color;
            p.a -= fadeInSpeed;
            c.a -= fadeInSpeed;
            text.color = c;
            text2.color = c;
            text3.color = c;
            text4.color = c;
            image.color = c;
            image2.color = c;
            image3.color = c;
            image4.color = c;
            image5.color = c;
            this.GetComponent<Image>().color = p;
            if (c.a <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
