using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public GameObject RangeObject;

    public float startTime;
    public float endTime;
    public float spawnRate;

    private BoxCollider rangeCollider;

    private void Awake()
    {
        rangeCollider = RangeObject.GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", startTime, spawnRate);
        Invoke("CancelInvoke", endTime);
    }

    void Spawn()
    {
        System.Random rand = new System.Random();
        int randomIndex = rand.Next(0,3);
        Instantiate(enemyPrefabs[randomIndex], RandomPosition(), Quaternion.identity);
    }

    Vector3 RandomPosition()
    {
        Vector3 originalPosition = RangeObject.transform.position;
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        System.Random rand = new Random();
        
        range_X = rand.Next( (int)((range_X / 2) * -1), (int)(range_X / 2));
        range_Z = rand.Next( (int)((range_Z / 2) * -1), (int)(range_Z / 2));
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originalPosition + RandomPostion;
        return respawnPosition;
    }
    
    
}
