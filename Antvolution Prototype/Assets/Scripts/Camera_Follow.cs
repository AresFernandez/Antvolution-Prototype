﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{

    public GameObject target;

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = target.transform.position + offset;
    }
}
