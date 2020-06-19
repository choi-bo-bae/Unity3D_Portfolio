using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float h;
    private float v;
    
    public float speed = 25.0f; //이동 속도
    private CharacterController cc;     //캐릭터 컨트롤러


    private float gravity = -20f;   //중력
    private float velocityY;    //낙하 속도

    public float jumpPower = 10.0f; //점프 파워



    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        

        //Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        Vector3 moveDir = new Vector3(h, 0, v);
        moveDir.Normalize();

        moveDir = Camera.main.transform.TransformDirection(moveDir);

       

        //여기까지 이동
        
       
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            velocityY = 0;

        }
        else
        {
            velocityY += gravity * Time.deltaTime;
            moveDir.y = velocityY;
        }

        //충돌처리와 중력 적용

        if(Input.GetButtonDown("Jump"))
        {
            velocityY = jumpPower;
        }

        //점프

        cc.Move(moveDir * speed * Time.deltaTime);

        //캐릭터 컨트롤러를 이용한 충돌처리 중
    }


}
