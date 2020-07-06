using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRotate : MonoBehaviour
{
    public float rotateSpeed = 10;

    private float angleX;


    
    // Update is called once per frame
    void Update()
    {
       
        if (this.gameObject.GetComponent<PlayerMove>().state != PlayerMove.PlayerState.Die)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        float h = Input.GetAxis("Mouse X");

        if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //모바일 용 터치가 뭐지??
        }


        angleX += h * rotateSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, angleX, 0);
    }
}
