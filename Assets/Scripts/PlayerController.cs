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
    
    Animator anim;

    private Vector3 moveVec;
    Vector3 rot = Vector3.zero;
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        gameObject.transform.eulerAngles = rot;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CheckKey();
        //gameObject.transform.eulerAngles = rot;
        
        CheckInput();
        Move();
        Turn();
        Roll();
        Close();
        SwapBullet();
        Shoot();
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
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
        if (isRoll || isClose)
        {
            moveVec = Vector3.zero;
        }

        transform.position += moveVec * speed * Time.deltaTime;
        anim.SetBool("Walk_Anim", moveVec != Vector3.zero);
    }

    void Turn()
    {
        //키보드 회전
        transform.LookAt(transform.position + moveVec);
        
        //마우스 회전
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * mouseRotationSpeed, 0);
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
            currentBulletIndex = 0;
            sDown2 = false;
        } else if (sDown2)
        {
            currentBulletIndex = 1;
            sDown1 = false;
        }
    }

    void Shoot()
    {
        if (isShooting)
        {
            GameObject bulletClone = Instantiate(bulletPrefabs[currentBulletIndex]);
            bulletClone.transform.position = shootTarget.transform.position;
            bulletClone.transform.rotation = shootTarget.transform.rotation;
        }
    }
}
