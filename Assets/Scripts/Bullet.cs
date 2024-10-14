using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : MonoBehaviour
{
    public int index;
    public float speed;
    public float destroyDelay;
    
    private int penetrationCount;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        if (index == 1)
        {
            penetrationCount = 1;
        } else if (index == 2)
        {
            penetrationCount = 2;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void Update()
    {
        destroyDelay -= Time.deltaTime;

        if (destroyDelay <= 0)
        {
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && penetrationCount > 0)
        {
            penetrationCount--;
            Debug.Log(collision.gameObject.name + " " + this.gameObject);
            Destroy(collision.gameObject);
            Debug.Log("Penetration Count: " + penetrationCount + ", " + this.gameObject);

            rigid.velocity *= 0.9f;
            if (penetrationCount <= 0)
            {
                Debug.Log("Destroyed By Shooting Enemy");
                Destroy(this.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Base"))
        {
            Debug.Log("Destroyed on the ground");
            Destroy(this.gameObject);
        }
    }
}
