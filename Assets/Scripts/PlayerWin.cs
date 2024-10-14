using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    Animator anim;
    private bool isRolling = false;
    
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!isRolling)
        {
            anim.SetBool("Roll_Anim", true);
            isRolling = true;
        } 

    }

    private void LateUpdate()
    {
        if (isRolling)
        {
            anim.SetBool("Roll_Anim", false);
        }
    }
}
