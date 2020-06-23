
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    enum EnemyState
    {
       Move,
       Attack,
       Die
    }

    #region "애너미 공통 변수"
    private CharacterController cc; //캐릭터 컨트롤러
    EnemyState state;   //상태
    private GameObject[] enemyTr;  //애너미의 순찰 포인트
    private GameObject target;  //플레이어
    public int hp = 100;    //체력
    private int idx;    //몇 번째 스폰포인트로 갈지 난수로 결정
    #endregion

    #region "Move상태에 필요한 변수들"
    public float speed = 20.0f;  // 이동속도
    public float searchRange = 1.0f;    //탐색 범위
    #endregion

    #region "Attack 상태에 필요한 변수들"
    public float attack = 5.0f; //공격력
    private float curTime = 0.0f;
    public float atkTime = 0.5f;    //일정 시간마다 공격하기
    #endregion

  

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        state = EnemyState.Move;    //기본적으로 순찰하기
        enemyTr = GameObject.FindGameObjectsWithTag("PatrolPoints");    //순찰 포인트 결정
        target = GameObject.Find("Player");
        idx = Random.Range(0, enemyTr.Length);  //몇 번째 순찰포인트로 갈지 결정
    }

    // Update is called once per frame
    void Update()
    {
        ChangeState();
    }

  
   

    private void Move()
    {
        state = EnemyState.Move;

        print(" Distance : " + Vector3.Distance(enemyTr[idx].transform.position, transform.position));  //디스턴스 찍어보기 왜인지 계속 Y값이 15가 나온다.

        if (Vector3.Distance(enemyTr[idx].transform.position, transform.position) <= 10.0f)   //순찰 포인트와 애너미가 가까워지면 다음 순찰포인트를 고른다.
        {
           
            idx = Random.Range(0, enemyTr.Length);  //난수 다시 결정
           
            Vector3 dir = (enemyTr[idx].transform.position - transform.position).normalized;    //이동
           
            cc.SimpleMove(dir * speed);
        }
        else                 //순찰포인트를 향해 ㄱㄱ
        {
            Vector3 dir = (enemyTr[idx].transform.position - transform.position).normalized;
            
            cc.SimpleMove(dir * speed);
        }


        //if (Vector3.Distance(enemyTr[idx].transform.position, transform.position) > 1.0f)            //순찰포인트를 향해 ㄱㄱ
        //{
        //    Vector3 dir = (enemyTr[idx].transform.position - transform.position).normalized;

        //    cc.SimpleMove(dir * speed);
        //}


        if (Vector3.Distance(target.transform.position, transform.position) < searchRange)   //탐색 범위 안에 들어오면 원거리에서 공격하기
        {
            state = EnemyState.Attack;
        }
        

    }

    private void Attack()
    {
        state = EnemyState.Attack;
    }

    IEnumerator Die()
    {
        state = EnemyState.Die;

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
        }
    }


    private void ChangeState()
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

}
