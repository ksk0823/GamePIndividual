using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostOneFSM : EnemyFSM
{
    public Transform target;  // 타겟 위치 참조
    public float attackRange = 6f;  // 공격 범위
    public int damage = 1; // 데미지
    
    public GameObject bulletPrefab;
    public ParticleSystem ashParticle; // 재가 날리는 파티클 시스템
    public Material dissolveMaterial;
    public float dissolveSpeed = 1.5f; // Dissolve 속도
    
    private float dissolveAmount = 0f; // 현재 Dissolve 상태
    private bool isDissolving = false; // Dissolve 시작 여부

    private SkinnedMeshRenderer[] meshs;
    private int deathCount = 2;
    private bool isDead = false;
    private Rigidbody rigid;
    private NavMeshAgent agent;    // NavMeshAgent 컴포넌트
    private float idleTime = 0.5f;  // Idle 상태 유지 시간
    private float idleTimer = 0f;  // Idle 상태 타이머
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 가져오기
        target = GameObject.FindGameObjectWithTag("Target").transform;  // Police Car 찾기
        rigid = GetComponent<Rigidbody>();
        ashParticle = GetComponent<ParticleSystem>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        Initialize();  // Initialize 메소드 호출
    }

    public override void Initialize() 
    {
        ashParticle.Pause();
        base.Initialize();
        agent.speed = 3f;  // 기본 속도 설정
        agent.stoppingDistance = attackRange; // 공격 범위에 도달하면 멈춤
    }

    // 상태 변경시 호출
    public override void OnStateEnter(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Idle: EnterIdle(); break;
            case MonsterState.ChaseCar: EnterChaseCar(); break;
            case MonsterState.SpeedUp: EnterSpeedUp(); break;
            case MonsterState.Attack: EnterAttack(); break;
            case MonsterState.Death: EnterDeath(); break;
        }
    }

    public override void OnStateExit(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Idle: ExitIdle(); break;
            case MonsterState.ChaseCar: ExitChaseCar(); break;
            case MonsterState.SpeedUp: ExitSpeedUp(); break;
            case MonsterState.Attack: ExitAttack(); break;
            case MonsterState.Death: ExitDeath(); break;
        }
    }

    public override void StateUpdate(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Idle: UpdateIdle(); break;
            case MonsterState.ChaseCar: UpdateChaseCar(); break;
            case MonsterState.SpeedUp: UpdateSpeedUp(); break;
            case MonsterState.Attack: UpdateAttack(); break;
            case MonsterState.Death: UpdateDeath(); break;
        }
    }

    public override void StateFixedUpdate(MonsterState state)
    {
        FreezeVelocity();
        // 필요한 상태에서만 물리 업데이트 수행
        if (state == MonsterState.ChaseCar || state == MonsterState.SpeedUp)
        {
            FixedUpdateMovement();
        }
    }

    // =====================================================
    // 2. 각 상태별 메소드 구현
    // =====================================================

    // Idle 상태
    private void EnterIdle()
    {
        idleTimer = 0f;
        agent.ResetPath();  // Idle 상태에서는 경로 초기화
    }

    private void UpdateIdle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            ChangeState(MonsterState.ChaseCar); // 0.5초 후 추격 시작
        }
    }

    private void ExitIdle()
    {
    }

    // ChaseCar 상태
    private void EnterChaseCar()
    {
        agent.enabled = true;
    }

    private void UpdateChaseCar()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            agent.enabled = false;
            Debug.Log("Target in range");
            ChangeState(MonsterState.Attack);  // 공격 범위 안에 들어옴
        }
        else
        {
            agent.enabled = true;
            agent.SetDestination(target.position);  // 플레이어를 목표로 설정
        }
    }

    private void ExitChaseCar()
    {
    }

    // SpeedUp 상태
    private void EnterSpeedUp()
    {
        if (agent.enabled == true)
        {
            agent.speed *= 2f;  // 속도 증가
        }
        else
        {
            agent.enabled = true;
            agent.speed *= 2f;  // 속도 증가
        }

    }

    private void UpdateSpeedUp()
    {
        agent.SetDestination(target.position);  // 플레이어 추적 유지
        float distance = Vector3.Distance(transform.position, target.position);
        
        if (distance <= attackRange)
        {
            ChangeState(MonsterState.Attack);
        }
    }

    private void ExitSpeedUp()
    {
    }

    // Attack 상태
    private void EnterAttack()
    {
        agent.speed = 0;
        agent.enabled = false;
        Debug.Log("Attack start!");
    }

    private void UpdateAttack()
    {
        if (CanAttackTarget())  // 공격이 유효한지 확인
        {
            // 적의 forward 방향으로 총알을 생성
            Vector3 spawnPosition = transform.position + transform.forward * 1f; 
            Quaternion spawnRotation = Quaternion.LookRotation(transform.forward); 
            
            GameObject bulletClone = Instantiate(bulletPrefab, spawnPosition, spawnRotation);
            // 여기서 공격 로직을 추가 (ex 데미지 계산)
            Debug.Log("Attack successful!");
            ChangeState(MonsterState.Death);
        }
        else
        {
            ChangeState(MonsterState.ChaseCar);
        }
    }

    private void ExitAttack()
    {
        Debug.Log("Attack finished.");
    }
    
    // Raycast로 목표가 시야에 있는지 확인
    private bool CanAttackTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        
        Debug.Log(distance);

        if (distance <= attackRange)
        {
            agent.enabled = false;
            Debug.Log("Police car in sight!");
            return true;
        }
        
        return false;
    }

    // Death 상태
    private void EnterDeath()
    {
        agent.enabled = false;  // 사망 시 이동 멈춤
        
        
        // 파티클 시스템 실행
        if (ashParticle != null)
        {
            ashParticle.Play();
        }
        
        // Dissolve 효과 시작
        foreach (var mesh in meshs)
        {
            mesh.material = dissolveMaterial;  // 머티리얼 변경
        }

        isDissolving = true;  // Dissolve 시작 플래그

        //Destroy(gameObject, 1f);  // 몬스터 비활성화
    }

    private void UpdateDeath()
    {
        
        dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime * dissolveSpeed);
        foreach (var mesh in meshs)
        {
            mesh.material.SetFloat("_DissolveAmount", dissolveAmount);  // 셰이더 값 업데이트
        }

        // Dissolve가 완료되면 오브젝트 제거
        if (dissolveAmount >= 1f)
        {
            Destroy(gameObject);  // 유령 제거
        }
        
    }

    private void ExitDeath()
    {
        // Death 상태에서는 Exit 로직이 필요하지 않음
    }

    // =====================================================
    // 3. 물리 업데이트 (ChaseCar 및 SpeedUp에서 사용)
    // =====================================================
    private void FixedUpdateMovement()
    {
       agent.Move(agent.velocity * Time.fixedDeltaTime);  // NavMesh 경로를 따라 이동
    }
    
    void FreezeVelocity()
    {
        if (!isDead)
        {
            rigid.angularVelocity = Vector3.zero;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (!isDead)
            {
                Vector3 reactVec = transform.position - collision.transform.position;
                
                StartCoroutine(OnDamage(reactVec));
            }
        }
    }
    
    // 플레이어의 총알에 맞으면 뒤로 살짝 밀려나도록
    IEnumerator OnDamage(Vector3 reactDirection)
    {
        this.deathCount--;
        
        if (deathCount == 1)
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.yellow;
            }
            ChangeState(MonsterState.SpeedUp);
        } else if (deathCount <= 0)
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.yellow;
            }
            isDead = true;
            GameManager.instance.DecreaseGoal();
            ChangeState(MonsterState.Death);
        }
        
        yield return new WaitForSeconds(0.1f);
        if (deathCount == 1)
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.red;
            }
        } else if (deathCount <= 0)
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.white;
            }
        }
        
        reactDirection = reactDirection.normalized;
        reactDirection += Vector3.up * 0.5f;
        rigid.AddForce(reactDirection * 3, ForceMode.Impulse);
        
    }
}
