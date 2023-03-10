using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    public string key;
    // Update is called once per frame
    
    public void StartTimer()
    {
        Debug.Log(key);
        Pooler.instance.DelayedDepop(2, key, this.gameObject);
    }
    
    void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0f;
    }
}
