using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private GameObject target;

    public float followSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
       
        target = GameObject.Find("CamFollowPoint");
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            target = GameObject.Find("1stView");    //1인칭
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            target = GameObject.Find("CamFollowPoint"); //3인칭
        }

        transform.position = target.transform.position;
    }


}
