using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotateSpeed = 30;

    private float angleX;


    
    // Update is called once per frame
    void Update()
    {
        PlayerMove state = GameObject.Find("Player").GetComponent<PlayerMove>();
        if (state.state != PlayerMove.PlayerState.Die)
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
