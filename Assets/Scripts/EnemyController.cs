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
    public int damage;
    public Transform target;

    private Rigidbody rigid;

    private bool isDead = false;
    private Vector3 moveDirection;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target").transform;
        
        Destroy(gameObject, destroyDelay);
    }

    void FixedUpdate()
    {
        FreezeVelocity();
        MoveToTarget();
        // 물리 기반 이동
        //rigid.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    void MoveToTarget()
    {
        if (!isDead && target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rigid.velocity = direction * speed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (!isDead)
            {
                Vector3 reactVec = transform.position - collision.transform.position;
                isDead = true;
                GameManager.instance.DecreaseGoal();
                StartCoroutine(OnDamage(reactVec));
            }
        }

        if (collision.gameObject.CompareTag("Target"))
        {
            GameManager.instance.DecreaseHP(damage);
            Destroy(this.gameObject);
        }
    }
    
    IEnumerator OnDamage(Vector3 reactDirection)
    {
        isDead = true;
        yield return new WaitForSeconds(0.1f);
        
        reactDirection = reactDirection.normalized;
        reactDirection += Vector3.up;
        rigid.AddForce(reactDirection * 5, ForceMode.Impulse);
        
        Destroy(this, 5);
        
    }
    
    void FreezeVelocity()
    {
        if (!isDead)
        {
            rigid.angularVelocity = Vector3.zero;
        }
        
    }

}
