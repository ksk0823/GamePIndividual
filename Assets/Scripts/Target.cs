using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int maxHp = 15;
    [SerializeField]
    private int currentHp;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            int damage = other.gameObject.GetComponent<EnemyController>().damage;
            currentHp-= damage; 
            GameManager.instance.DecreaseHP(damage);
            Destroy(other.gameObject, 0.5f);
        }
        
    }
}
