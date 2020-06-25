
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
    #endregion

    #region "Move상태에 필요한 변수들"
    public float speed = 20.0f;  // 이동속도
    public float searchRange = 50.0f;    //탐색 범위
    private GameObject[] enemyTr;  //애너미의 순찰 포인트
    private int idx;    //몇 번째 스폰포인트로 갈지 난수로 결정
    #endregion

    #region "Attack 상태에 필요한 변수들"
    public float attack = 5.0f; //공격력
    private float rayDis = 10.0f;
    public GameObject bulletEffectFactory;  //총알 이펙트
    private GameObject target;  //플레이어
    public int hp = 100;    //체력
    #endregion

    #region "Damaged에 필요한 변수들"

    #endregion

    #region "Die에 필요한 변수들"

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        state = EnemyState.Move;    //기본적으로 순찰하기
        //anim.SetTrigger("Move");
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
        state = EnemyState.Move;

        //anim.SetTrigger("Move");

        if (Vector3.Distance(target.transform.position, transform.position) < searchRange)   //탐색 범위 안에 들어오면 원거리에서 공격하기
        {
            state = EnemyState.Attack;

            anim.SetTrigger("Attack");
        }
        if (Vector3.Distance(enemyTr[idx].transform.position, transform.position) <= 1.0f)   //순찰 포인트와 애너미가 가까워지면 다음 순찰포인트를 고른다.
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
        state = EnemyState.Attack;

        anim.SetTrigger("Attack");

        Vector3 dir = target.transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5.0f * Time.deltaTime);
        dir.Normalize();

      
           
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
               
                Debug.DrawRay(ray.origin, hitInfo.transform.position - transform.position, Color.blue, 0.3f);

                GameObject bulletEffect = Instantiate(bulletEffectFactory);

                bulletEffect.transform.position = hitInfo.point;

                bulletEffect.transform.forward = hitInfo.normal;

                //if (hitInfo.transform.name.Contains("Player"))
                //{
                //    PlayerMove player = hitInfo.collider.gameObject.GetComponent<PlayerMove>();
                //    //player.HitDamage(10);
                //}
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDis, Color.red, 0.3f);

            }

    }



    IEnumerator Die()
    {
        state = EnemyState.Die;

        //폭발이펙트
        ScoreManager score = GameObject.Find("ScoreMgr").gameObject.GetComponent<ScoreManager>();

        score.RemainCount--;
        //디스트로이

        Destroy(gameObject, 2.0f);
        yield return 0;
    }



    public void HitDamage(int damage)
    {
        if (state == EnemyState.Die) return;

        if (hp > 0)
        {
            hp -= damage;
            anim.SetTrigger("Attack");
        }
        else
        {
            StartCoroutine(Die());
            anim.SetTrigger("Die");
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }


}
