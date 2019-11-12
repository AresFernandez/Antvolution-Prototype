using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeIn : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float fadeInSpeed = 0.01f;
    private Color c;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        c = text.color;
        c.a += fadeInSpeed;
        text.color = c;
    }
}
