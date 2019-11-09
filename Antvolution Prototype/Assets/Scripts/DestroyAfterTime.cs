using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float dieTime = 1f;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= dieTime)
        {
            Destroy(this.gameObject);
        }
    }
}
