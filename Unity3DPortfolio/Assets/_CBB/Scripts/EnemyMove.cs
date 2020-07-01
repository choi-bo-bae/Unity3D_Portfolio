
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    enum EnemyState
    {
       Move,
       Attack,
       Damaged,
       Die
    }

    #region "애너미 공통 변수"
    private CharacterController cc; //캐릭터 컨트롤러
    EnemyState state;   //상태
    private Animator anim;  //애너미 애니메이션
    public GameObject rifle;
    #endregion

    #region "Move상태에 필요한 변수들"
    public float speed = 20.0f;  // 이동속도
    public float searchRange = 50.0f;    //탐색 범위
    private GameObject[] enemyTr;  //애너미의 순찰 포인트
    private int idx;    //몇 번째 스폰포인트로 갈지 난수로 결정
    #endregion

    #region "Attack 상태에 필요한 변수들"
    public int attack = 10; //공격력
    private float rayDis = 10.0f;
    public GameObject bulletEffectFactory;  //총알 이펙트
    private GameObject target;  //플레이어
    private float curTime = 0.0f;
    private float atkTime = 1.0f;
    public int hp = 100;    //체력
    public GameObject gunFlashFactory; //플래시
    public GameObject flashPoint;  //빛나는 포인트
    public GameObject firePoint;
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        state = EnemyState.Move;    //기본적으로 순찰하기
        enemyTr = GameObject.FindGameObjectsWithTag("PatrolPoints");    //순찰 포인트 결정
        target = GameObject.Find("Player"); //보통 때는 순찰포인트를 향해 가다가 플레이어가 일정범위 이상 들어오면 공격
        idx = Random.Range(0, enemyTr.Length);  //몇 번째 순찰포인트로 갈지 결정
        anim = GetComponentInChildren<Animator>();
       
    }



    // Update is called once per frame
    void Update()
    {
        ChangeState();

        
    }



    private void ChangeState()  //상태 변경
    {
        switch (state)
        {
            case EnemyState.Move:
                Move();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Die:
                Die();
                break;
        }
    }


    private void Move()
    {
       
        anim.SetTrigger("Move");

        if (Vector3.Distance(target.transform.position, transform.position) < searchRange)   //탐색 범위 안에 들어오면 원거리에서 공격하기
        {
            state = EnemyState.Attack;

        }
        else if (Vector3.Distance(enemyTr[idx].transform.position, transform.position) <= 1.0f)   //순찰 포인트와 애너미가 가까워지면 다음 순찰포인트를 고른다.
        {
            idx = Random.Range(0, enemyTr.Length);  //난수 다시 결정          
        }
        else
        {
            Vector3 dir = enemyTr[idx].transform.position - transform.position;    //이동
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5.0f * Time.deltaTime);
            dir.Normalize();
            cc.SimpleMove(dir * speed);
        }
    }

    private void Attack()
    {
       
        anim.SetTrigger("Attack");


        if (Vector3.Distance(target.transform.position, transform.position) > searchRange)   //탐색 범위 안에 들어오면 원거리에서 공격하기
        {
            state = EnemyState.Move;

        }

        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5.0f * Time.deltaTime);
        

       
        curTime += Time.deltaTime;

        if (curTime > atkTime)
        {
            
            Ray ray = new Ray(firePoint.transform.position, firePoint.transform.forward);
            RaycastHit hitInfo;

            
            GameObject flash = Instantiate(gunFlashFactory);//총이 발사되는 부분에서 플래시 터지는 이펙트
            gunFlashFactory.transform.position = flashPoint.transform.position;
            gunFlashFactory.transform.forward = flashPoint.transform.forward;
           

            if (Physics.Raycast(ray, out hitInfo))
            {
               
                Debug.DrawRay(ray.origin, hitInfo.point - firePoint.transform.position, Color.blue, 0.3f);

                GameObject bulletEffect = Instantiate(bulletEffectFactory);//총알이(레이가) 부딫히는 부분 이펙트
                bulletEffect.transform.position = hitInfo.point;
                bulletEffect.transform.forward = hitInfo.normal;
                

                if (hitInfo.transform.name.Contains("Player"))
                {
                    PlayerMove player = hitInfo.collider.gameObject.GetComponent<PlayerMove>();
                    player.HitDamage(attack);
                  
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDis, Color.red, 0.3f);

            }
              

            curTime = 0.0f;
            
        }

       



    }



    IEnumerator Die()
    {
       
        anim.SetTrigger("Die");
        ScoreManager score = GameObject.Find("ScoreMgr").gameObject.GetComponent<ScoreManager>();

        score.RemainCount--;

        Destroy(gameObject, 2.0f);
        //디스트로이
        yield return 0;
    }



    public void HitDamage(int damage)
    {
        if (state == EnemyState.Die) return;

        if (hp > 0)
        {
            hp -= damage;
           
        }
        else
        {
            StartCoroutine(Die());
            state = EnemyState.Die;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }


}
