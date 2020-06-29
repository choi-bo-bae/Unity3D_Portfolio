using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    enum PlayerState
    {
        Idle,
        KnifeIdle,
        Move,
        KnifeModeMove,
        KnifeAttack,
        Damaged,
        Die
    }

    //공통
    private CharacterController cc;     //캐릭터 컨트롤러
    private float gravity = -20f;   //중력
    private float velocityY = 0.0f;    //낙하 속도 
    public float Hp = 100.0f;   //체력
    PlayerState state;  //상태 쳌크
    private Animator anim;  //플레이어 애니메이션

    //Move
    private float h;
    private float v;
    public float speed = 25.0f; //이동 속도
    public float jumpPower = 10.0f; //점프 파워
    private bool isTouch = false;
    private float radius;   //조이스틱 배경 반지름
    private Vector3 movePosition;   //얼마만큼 움직여라
    private int jumpCount = 0;  //1단점프 까지 가능
    [SerializeField] private RectTransform rect_backGround; //조이스틱 배경
    [SerializeField] private RectTransform rect_Joystick;   //조이스틱

  

    //Start is called before the first frame update
    void Start()
    {
       radius = rect_backGround.rect.width * 0.5f;
       cc = GetComponent<CharacterController>();
       anim = GetComponentInChildren<Animator>();
       state = PlayerState.Idle;   //기본적으로 아이들 상태
    }

    private void changeState()
    {
        //PlayerChangeWeapon GunMode = GetComponent<PlayerChangeWeapon>();
        //PlayerChangeWeapon KnifeMode = GetComponent<PlayerChangeWeapon>();

        
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
        }
    }

    


    // Update is called once per frame
    void Update()
    {
       
        Move();
        
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (h == 0 && v == 0)
        {
            if (isTouch)
            {
                transform.localPosition += movePosition; //플레이어를 움직여라
            }
        }

        changeState();
        
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

        //Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        Vector3 moveDir = new Vector3(h, 0, v);
        moveDir.Normalize();

        moveDir = Camera.main.transform.TransformDirection(moveDir);

        PlayerChangeWeapon GunMode = GetComponent<PlayerChangeWeapon>();
        if (GunMode.ShotGun.activeSelf == true)
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
           
            if (GunMode.ShotGun.activeSelf == true)
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

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        Vector3 moveDir = new Vector3(h, 0, v);
        moveDir.Normalize();

        moveDir = Camera.main.transform.TransformDirection(moveDir);

        if (h == 0 && v == 0)
        {
            PlayerChangeWeapon GunMode = GetComponent<PlayerChangeWeapon>();
            if (GunMode.ShotGun.activeSelf == true)
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


    

    //========================================================================여기까지 이동

    IEnumerator Damaged()
    {
        state = PlayerState.Damaged;
        anim.SetTrigger("Damaged");

        yield return 0;
    }

    public void KnifeAttack()
    {
        PlayerChangeWeapon GunMode = GetComponent<PlayerChangeWeapon>();
        if (GunMode.ShotGun.activeSelf == false)
        {
            anim.SetTrigger("KnifeAttack");
        }
        
    }

    private void OnTriggerEnter(GameObject hitChild, GameObject hitOther)   //나이프로 뚜까
    {
        if(hitOther.transform.name.Contains("Enemy"))
        {
            EnemyMove enemy = hitOther.gameObject.GetComponent<EnemyMove>();
            enemy.HitDamage(10);
        }
    }
  

    public void OnDrag(PointerEventData eventData)  //조이스틱 조작 중
    {
        Vector2 value = eventData.position - (Vector2)rect_backGround.position;

        value = Vector2.ClampMagnitude(value, radius);

        rect_Joystick.localPosition = value;

        float distance = Vector2.Distance(rect_backGround.position, rect_Joystick.position) / radius;
        value = value.normalized;
        movePosition = new Vector3(value.x * speed * distance * Time.deltaTime, 0f, value.y * speed * distance * Time.deltaTime);

    }

    public void OnPointerDown(PointerEventData eventData)   //조이스틱 클릭 함
    {
        isTouch = true;

    }

    public void OnPointerUp(PointerEventData eventData) //조이스틱 조작 종료
    {
        isTouch = false;
        rect_Joystick.localPosition = Vector3.zero;
        movePosition = Vector3.zero;
    }

   

    public void HitDamage(int value)
    {
        Hp -= value;
        anim.SetTrigger("Damaged");
    }


}
