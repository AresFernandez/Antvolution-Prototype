using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInImage : MonoBehaviour
{
    public Image image;
    public float fadeInSpeed = 0.01f;
    public float fadeAfterTime = 5.0f;
    private Color c;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= fadeAfterTime)
        {
            c = image.color;
            c.a += fadeInSpeed;
            image.color = c;
            if (c.a >= 1)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
