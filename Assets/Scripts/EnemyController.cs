using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float destroyDelay;

    private Rigidbody rigid;

    

    private Vector3 moveDirection;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // 랜덤한 방향 설정
        moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Destroy(gameObject, destroyDelay);
    }

    void FixedUpdate()
    {
        // 물리 기반 이동
        rigid.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);
    }
    
    /*
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log(collision.gameObject.name);
            Destroy(this.gameObject);
        }
    }
    */
}
