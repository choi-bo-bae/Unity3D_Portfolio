using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private float h;
    private float v;

    public float speed = 25.0f; //이동 속도
    
    private CharacterController cc;     //캐릭터 컨트롤러
    
    private float gravity = -20f;   //중력
    private float velocityY;    //낙하 속도

    public float jumpPower = 10.0f; //점프 파워

    private bool isTouch = false;
    
    private float radius;   //조이스틱 배경 반지름

    private Vector3 movePosition;   //얼마만큼 움직여라

    private bool jumpClick = false; //버튼을 눌러 점프했는가

    private int jumpCount = 0;  //2단점프 까지 가능


    [SerializeField] private RectTransform rect_backGround; //조이스틱 배경
    [SerializeField] private RectTransform rect_Joystick;   //조이스틱

   
    //Start is called before the first frame update
    void Start()
    {
       radius = rect_backGround.rect.width * 0.5f;
       cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        jumpClick = false;

        if (h == 0 && v == 0)
        {
            if (isTouch)
            {
               transform.localPosition += movePosition; //플레이어를 움직여라
            }
        }
    }

    private void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");



        // Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        Vector3 moveDir = new Vector3(h, 0, v);
        moveDir.Normalize();

        moveDir = Camera.main.transform.TransformDirection(moveDir);



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

        if (jumpCount < 2)
        {
            if (Input.GetButtonDown("Jump") || jumpClick == true)
            {
                velocityY = jumpPower;
                jumpCount++;
            }
        }

        //점프

        cc.Move(moveDir * speed * Time.deltaTime);

        //캐릭터 컨트롤러를 이용한 충돌처리 중
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


    public void OnFireButton()
    {
        
    }

    public void OnJumpButton()
    {
        jumpClick = true;
       
    }


}
