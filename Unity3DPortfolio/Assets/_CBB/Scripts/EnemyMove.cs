
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
    private int idx;
    #endregion

    #region "Move상태에 필요한 변수들"
    public float speed = 40.0f;  // 이동속도
    public float searchRange = 1.0f;    //탐색 범위
    #endregion

    #region "Attack 상태에 필요한 변수들"
    public float attack = 5.0f; //공격력
    private float curTime = 0.0f;
    public float atkTime = 0.5f;
    #endregion

  

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        state = EnemyState.Move;
        enemyTr = GameObject.FindGameObjectsWithTag("PatrolPoints");
        target = GameObject.Find("Player");
        idx = Random.Range(0, enemyTr.Length);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeState();
    }

  
   

    private void Move()
    {
        state = EnemyState.Move;

        print(" Distance : " + Vector3.Distance(enemyTr[idx].transform.position, transform.position));

        if (Vector3.Distance(enemyTr[idx].transform.position, transform.position) <= 3.0f)   //순찰 포인트와 애너미가 가까워지면 다음 순찰포인트를 고른다.
        {
           
            idx = Random.Range(0, enemyTr.Length);
           
            Vector3 dir = (enemyTr[idx].transform.position - transform.position).normalized;
           
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
