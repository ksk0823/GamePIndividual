using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int maxHp = 15;
    private MeshRenderer mesh;
    public GameObject electricCapsule;
    public AudioClip electricSound;
    [SerializeField]
    private int currentHp;

    private void Awake()
    {
        electricCapsule.SetActive(false);
        mesh = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            int damage = other.gameObject.GetComponent<EnemyBullet>().index;
            currentHp-= damage; 
            GameManager.instance.DecreaseHP(damage);
            GameManager.instance.playAudio(electricSound);
            Destroy(other.gameObject, 0.5f);
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        electricCapsule.SetActive(true);
        mesh.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        mesh.material.color = Color.white;
        electricCapsule.SetActive(false);
    }
}
