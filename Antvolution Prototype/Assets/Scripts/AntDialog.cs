using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AntDialog : MonoBehaviour
{
    public Text text;
    public float fadeInSpeed = 0.05f;
    public float minimumTime = 2.0f;
    public Image image;
    private Color c;
    private Color i;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= minimumTime)
        {
            c = text.color;
            c.a -= fadeInSpeed;
            text.color = c;
            i = image.color;
            i.a -= fadeInSpeed;
            image.color = i;
            if (c.a <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
        else if (c.a < 1)
        {
            c = text.color;
            c.a += fadeInSpeed;
            text.color = c;
            i = image.color;
            i.a += fadeInSpeed;
            image.color = i;
        }
    }

    private void OnEnable()
    {
        startTime = Time.time;
    }
}
