using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float mouseRotationSpeed;
    public List<GameObject> bulletPrefabs = new List<GameObject>();
    public GameObject shootTarget;
    
    private float hAxis;
    private float vAxis;
    private int currentBulletIndex = 0;
    
    private bool isWalk;
    private bool isRoll;
    private bool isOpen;
    private bool isClose;
    private bool isShooting;
    private bool sDown1;
    private bool sDown2;
    private bool isBorder;
    private bool pressedEsc;
    public bool pauseGame;

    [Header("Sounds")]
    public AudioClip shootOne;
    public AudioClip shootTwo;
    public AudioClip swapSound;
    
    Animator anim;
    private Rigidbody rigid;

    private Vector3 moveDir;
    private Vector3 moveVec;
    Vector3 rot = Vector3.zero;
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.eulerAngles = rot;
        pauseGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseGame)
        {
            return;
        }
        //CheckKey();
        //gameObject.transform.eulerAngles = rot;
        
        CheckInput();
        //Move();
        Turn();
        Roll();
        Close();
        SwapBullet();
        Shoot();
        CheckPause();
    }

    private void FixedUpdate()
    {
        Move();
        FreezeRotation();
        StopToWall();
    }

    void CheckInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        isRoll = Input.GetKeyDown(KeyCode.Space);
        isClose = Input.GetKeyDown(KeyCode.C);
        isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        pressedEsc = Input.GetKeyDown(KeyCode.Escape);
    }

    void Move()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
        if (isRoll || isClose)
        {
            moveVec = Vector3.zero;
        }

        // 플레이어의 현재 바라보는 방향으로 이동
        Vector3 moveDirection = transform.forward * moveVec.z + transform.right * moveVec.x;
        //transform.position += moveDirection * speed * Time.deltaTime;
        if (!isBorder)
        {
            rigid.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);
        }
        anim.SetBool("Walk_Anim", moveVec != Vector3.zero);
    }

    void Turn()
    {
        //마우스 회전
        float mouseX = Input.GetAxis("Mouse X") * mouseRotationSpeed;
        transform.Rotate(0, mouseX * Time.deltaTime, 0);
    }

    void Roll()
    {
        if (isRoll)
        {
            if (anim.GetBool("Roll_Anim"))
            {
                anim.SetBool("Roll_Anim", false);
            }
            else
            {
                anim.SetBool("Roll_Anim", true);
            }
        }
    }

    void Close()
    {
        if (isClose)
        {
            if (!anim.GetBool("Open_Anim"))
            {
                anim.SetBool("Open_Anim", true);
            }
            else
            {
                anim.SetBool("Open_Anim", false);
            }
        }
    }

    void SwapBullet()
    {
        if (sDown1)
        {
            GameManager.instance.playAudio(swapSound);
            currentBulletIndex = 0;
            sDown2 = false;
        } else if (sDown2)
        {
            GameManager.instance.playAudio(swapSound);
            currentBulletIndex = 1;
            sDown1 = false;
        }
    }

    void Shoot()
    {
        if (isShooting)
        {
            if (currentBulletIndex == 0)
            {
                GameManager.instance.playAudio(shootOne);
            }
            else
            {
                GameManager.instance.playAudio(shootTwo);
            }
            GameObject bulletClone = Instantiate(bulletPrefabs[currentBulletIndex], shootTarget.transform.position, shootTarget.transform.rotation);
        }
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void CheckPause()
    {
        if (pressedEsc)
        {
            GameManager.instance.playAudio(swapSound);
            pauseGame = true;
            GameManager.instance.pauseGame();
        }
    }
    
    
}
