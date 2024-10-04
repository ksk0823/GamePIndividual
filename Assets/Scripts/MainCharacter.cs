using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public int hp;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        this.hp = 3;
        this.speed = 3.0f;
        print("HP:" + hp +"\n");
        print("Speed:" + speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
