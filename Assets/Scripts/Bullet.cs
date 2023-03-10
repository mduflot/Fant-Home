using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0f;
    }

    private void OnBecameInvisible()
    {
        Pooler.instance.Depop("Bullet", this.gameObject);
    }
}
