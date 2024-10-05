using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float destroyDelay;

    private Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        // 랜덤한 방향 설정
        moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        
        Destroy(gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
}
