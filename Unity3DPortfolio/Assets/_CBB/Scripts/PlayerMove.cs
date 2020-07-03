using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;


[System.Serializable]
public struct PlayerSfx
{
    public AudioClip walk;
    public AudioClip knife;
    public AudioClip die;
    
}


public class PlayerMove : MonoBehaviour
{

    public enum PlayerState
    {
        Idle,
        KnifeIdle,
        Move,
        KnifeModeMove,
        KnifeAttack,
        Damaged,
        Die
    }

    #region "공통"
    private CharacterController cc;     //캐릭터 컨트롤러
    private float gravity = -20f;   //중력
    private float velocityY = 0.0f;    //낙하 속도 
    public float Hp = 100.0f;   //체력
    public PlayerState state;  //상태 쳌크
    private Animator anim;  //플레이어 애니메이션
    public Vector2 margin;  //뷰포트 좌표로 이동
    public VariableJoystick joyStick; //조이스틱 public? private?
    public GameObject enemy;
    public PlayerSfx playerSfx;
    #endregion


    #region "Move"
    private float h;
    private float v;
    public float speed = 25.0f; //이동 속도
    public float jumpPower = 10.0f; //점프 파워
    private Vector3 movePosition;   //얼마만큼 움직여라
    private int jumpCount = 0;  //1단점프 까지 가능
    AudioSource audio;
    private float footStepDelay = 0.0f;
    #endregion

    #region "Die"
    public Text retryTxt;
    public GameObject retry;
    public GameObject quit;
    #endregion

    #region "Danaged"
    public Image hpBar;
    #endregion

    //Start is called before the first frame update
    void Start()
    {
      
       cc = GetComponent<CharacterController>();
       anim = GetComponentInChildren<Animator>();
       state = PlayerState.Idle;   //기본적으로 아이들 상태
       retryTxt.enabled = false;
       retry.SetActive(false);
       quit.SetActive(false);
       margin = new Vector2(0.08f, 0.05f);
        audio = GetComponent<AudioSource>();
    }

   
    private void changeState()
    {
       
        switch (state)
        {
            case PlayerState.Idle:
                Idle();            
                break;
            case PlayerState.KnifeIdle:
                KnifeIdle();
                break;
            case PlayerState.Move:
                
                Move();
                break;
            case PlayerState.KnifeModeMove:
               
                KnifeModeMove();
                break;
            case PlayerState.KnifeAttack:
                
                KnifeAttack();
                break;
            case PlayerState.Damaged:
                StartCoroutine(Damaged());
                break;
            case PlayerState.Die:
                StartCoroutine(Dead());
                break;
        }
    }

    


    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = Hp * 0.01f;
       

        if (state != PlayerState.Die)
        {
            Move();
            

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            
            changeState();
        }

    }


    public void Jump()  //점프
    {
       
        if (jumpCount < 1)
        {
            velocityY = jumpPower;
            jumpCount++;
        }
       
    }

    private void Idle()
    {
        anim.SetTrigger("Idle");

       
        if (h != 0 || v != 0)
        {
            state = PlayerState.Move;
           
        }
    }

    private void KnifeIdle()
    {
        anim.SetTrigger("KnifeIdle");

        if (h != 0 || v != 0)
        {
            state = PlayerState.KnifeModeMove;

        }
    }

    //=======================================================================================================여기까지 아이들 상태


   
    private void Move() //건 모드 --> 애니메이션 때문에 나눴음
    {
       
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        
        if (h == 0 && v == 0)
        {
            h = joyStick.Horizontal;
            v = joyStick.Vertical;
        }


        Vector3 moveDir = new Vector3(h, 0, v);
        moveDir.Normalize();

        moveDir = Camera.main.transform.TransformDirection(moveDir);

        if (h != 0 || v != 0)
        {
            footStepDelay += Time.deltaTime;
            if (footStepDelay >= 0.6f)
            {
                audio.PlayOneShot(playerSfx.walk);
                footStepDelay = 0.0f;
            }
        }


        if (this.gameObject.GetComponent<PlayerChangeWeapon>().ShotGun.activeSelf == true)  //건 모드인지 체크
        {
            if (v >= 0.1f)
            {
                anim.SetTrigger("FMove");
            }
            if (v <= -0.1f)
            {
                anim.SetTrigger("BMove");
            }
            if (h >= 0.1f)
            {
                anim.SetTrigger("RMove");
            }
            if (h <= -0.1f)
            {
                anim.SetTrigger("LMove");
            }
        }
        

        if (h == 0 && v == 0)
        {

            if (this.gameObject.GetComponent<PlayerChangeWeapon>().ShotGun.activeSelf == true)  //건 모드인지 체크
            {
                state = PlayerState.Idle;
            }
            else
            {

                state = PlayerState.KnifeIdle;
            }
           
        }

        //여기까지 이동
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            velocityY = 0;
            jumpCount = 0;
        }
        else
        {
            velocityY += gravity * Time.deltaTime;
            moveDir.y = velocityY;
        }

        //충돌처리와 중력 적용
        

        cc.Move(moveDir * speed * Time.deltaTime);
        
        //캐릭터 컨트롤러를 이용한 충돌처리 중
    }


    private void KnifeModeMove()    //나이프 모드
    {
        anim.SetTrigger("KnifeModeMove");

        Move();
       
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");


        //if (h == 0 && v == 0)
        //{
        //    h = joyStick.Horizontal;
        //    v = joyStick.Vertical;
        //}

        //Vector3 moveDir = new Vector3(h, 0, v);
        //moveDir.Normalize();

        //moveDir = Camera.main.transform.TransformDirection(moveDir);

        //if (h != 0 || v != 0)
        //{
        //    footStepDelay += Time.deltaTime;
        //    if (footStepDelay >= 0.6f)
        //    {
        //        audio.PlayOneShot(playerSfx.walk);
        //        footStepDelay = 0.0f;
        //    }
        //}

        //if (h == 0 && v == 0)
        //{
        //    if (this.gameObject.GetComponent<PlayerChangeWeapon>().ShotGun.activeSelf == true)  //건 모드인지 체크
        //    {
        //        state = PlayerState.Idle;
        //    }
        //    else
        //    {

        //        state = PlayerState.KnifeIdle;
        //    }

        //}

        ////여기까지 이동
        //if (cc.collisionFlags == CollisionFlags.Below)
        //{
        //    velocityY = 0;
        //    jumpCount = 0;
        //}
        //else
        //{
        //    velocityY += gravity * Time.deltaTime;
        //    moveDir.y = velocityY;
        //}

        ////충돌처리와 중력 적용


        //cc.Move(moveDir * speed * Time.deltaTime);

        ////캐릭터 컨트롤러를 이용한 충돌처리 중


    }




    //========================================================================여기까지 이동

    IEnumerator Damaged()
    {
        state = PlayerState.Damaged;
        anim.SetTrigger("Damaged");

        yield return 0;
    }

    public void KnifeAttack()
    {
       
        anim.SetTrigger("KnifeAttack");
        state = PlayerState.KnifeAttack;
        audio.PlayOneShot(playerSfx.knife);
    }
    

    public void OnTriggerEnter(Collider other)  //나이프 모드 때 상태가 어택이면 애너미 오브젝트 피격
    {
        Debug.Log(" 플레이어가 나이프어택상태가 아님!");

        if (state != PlayerState.KnifeAttack) return;

        Debug.Log(" 피 깎기");
        EnemyMove enemyDamage = enemy.GetComponent<EnemyMove>();
            enemyDamage.HitDamage(10);


    }


   

    public void HitDamage(int value)
    {
        if (Hp > 0)
        {
            Hp -= value;
            anim.SetTrigger("Damaged");
        }

        if (Hp <= 0)
        {
            state = PlayerState.Die;
            StartCoroutine(Dead());
            anim.SetTrigger("Die");
           

        }

    }


    IEnumerator Dead()
    {
        state = PlayerState.Die;
        anim.SetTrigger("Die");

        audio.PlayOneShot(playerSfx.die);

        retryTxt.enabled = true;
        retry.SetActive(true);
        quit.SetActive(true);

        StopAllCoroutines();

        cc.enabled = false;

        yield return 0;
    }


}
