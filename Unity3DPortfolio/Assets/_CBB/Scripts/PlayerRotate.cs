using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRotate : MonoBehaviour, IDragHandler
{
    public float rotateSpeed = 10;

    private float angleX;

    public float yAngle = 0f;
    
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

      
        angleX += h * rotateSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, angleX, 0);
    }


   
    

}
