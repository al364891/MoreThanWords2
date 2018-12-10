﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowForce : MonoBehaviour {
    Quaternion rotation;
    float speed;
    float v;
    float g;
    public float force;
    public float angle;
    float x;
    float y;
    [SerializeField] Rigidbody2D rb;
    private bool launched;
    public bool updateRotation;
    public float lastAngle;

    private void Start()
    {
        //StartingAngle();
    }

    private void Awake()
    {
        updateRotation = true;
        launched = false;
        rb = this.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        //LaunchArrow();
    }

    // Update is called once per frame
    void Update () {
        if (launched)
        {
            UpdateAngle();
        }
    }

    void StartingAngle()
    {
        /*g = rb.gravityScale;
        v = rb.velocity.magnitude;
        angle = Mathf.Atan((-v * v + Mathf.Sqrt(v * v * (v * v + 2 * y * g) - g * g * x * x)) / (g * x));
        Debug.Log((-v * v + Mathf.Sqrt(v * v * (v * v + 2 * y * g) - g * g * x * x)) / (g * x));
        angle = Mathf.Rad2Deg*angle;*/
        Quaternion rotation = Quaternion.Euler(0, 0, angle);;
        this.transform.rotation *= rotation;
    }

    internal void UpdateLastAngle()
    {
        if (updateRotation)
        {
            lastAngle = angle;
            updateRotation = false;
        }
    }

    void LaunchArrow()
    {
        launched = true;
        rb.isKinematic = false;
        rb.velocity = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * force, Mathf.Sin(Mathf.Deg2Rad * angle) * force, 0);
    }

    void UpdateAngle()
    {
        if (updateRotation)
        {
            angle = Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x);
            rotation = Quaternion.Euler(0, 0, angle);
            this.transform.rotation = rotation;
        }
        else
        {
            rotation = Quaternion.Euler(0, 0, lastAngle);
            this.transform.rotation = rotation;
        }
    }

    public void SetAngle(float angle)
    {
        this.angle = angle;
        Quaternion rotation = Quaternion.Euler(0, 0, angle); ;
        this.transform.rotation *= rotation;
        //Debug.Log(angle);
        Invoke("LaunchArrow", 0.35f);
    }
}
