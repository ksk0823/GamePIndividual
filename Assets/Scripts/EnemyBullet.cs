using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int index;
    public float speed = 1f;
    public float destroyDelay = 1.5f;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        if (index == 1)
        {
            
        } else if (index == 2)
        {
            
        } else if (index == 3)
        {
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 총알이 생성된 방향으로 지속적으로 이동
        rigid.velocity = transform.forward * speed;
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
        if (collision.gameObject.CompareTag("Target"))
        {
            //GameManager.instance.DecreaseHP(index);
            Destroy(this.gameObject);
        }
    }
}
